#!groovy

// Build Params
Map buildParams = [ LogLevel                     : "ALL",
                    DevRoot                     : "d:/gitroot/WFM_dev",
                    MasterBuildVersion          : "15.2.0.${env.BUILD_ID}",
                    NodeToRun                   : "Build-Slaves-B",
                    DesktopLabel                : "VCD-Desktop${env.BUILD_TAG}",
                    MainRepoURL                 : "git@atlgit:DEVOPS_Test",
                    MainRepoBranch              : "15.2_HFR1",
                    defaultConfigFilePath       : "Jenkinsfile.yaml"] as HashMap

//Preparation
currentBuild.displayName = "${buildParams.MasterBuildVersion}"

@Library('devops-lib@15.2.0_alpha')
//TODO: change to below for official one
//@Library('CI_workflowLibs@15.2.0_alpha')

import com.verint.devops.core.*

def m_zLogger = new zLogger(steps, "ALL")
def m_zDevOpsHelper = new zDevOpsHelper(steps,buildParams, env, m_zLogger)

pipeline {

    agent {
        node {
            label "${buildParams.NodeToRun}"
            customWorkspace "${buildParams.DevRoot}"
        }
    }
    triggers {
        cron('@daily')
    }
    parameters {
        //Not too much value here
        string(name: 'EmailTo', defaultValue: 'hoai.tang@verint.com', description: 'Who should I mail to ?')
    }
    stages {
        stage("Prepare Environment") {
            steps {
                script {
                    m_zDevOpsHelper.loadLibDefaultConfig()
                }
            }
        }
        stage("Update Workspace") {
            steps {
                echo "Update Workspace:  ${env.NODE_NAME}"
                zSCMCheckout (m_zDevOpsHelper,m_zDevOpsHelper.getConfigData().DevRoot, "${m_zDevOpsHelper.getConfigData().MainRepoURL}", "${m_zDevOpsHelper.getConfigData().MainRepoBranch}")
                zLoadConfig (m_zDevOpsHelper,"Loading Team Config")
                zLoadConfig (m_zDevOpsHelper,"Loading Group Config")
                //TODO: if you want to inject the getKBs
//              zInstallBuilder (m_zDevOpsHelper,"Install Builder GetKB")
//              bat "exit 1"
            }
        }
stage("Build Bundle"){
    steps {
        script{
            //=== Create Bundle ===

                /// Get Target Release Version
                println  "zMasterBuildVersion=" +zMasterBuildVersion


                releaseName=getTargetReleaseVersion("git@atlgit:DEVOPS_Configs","master","GetTargetVersion\\wfm\\build_config\\reference_tag","${zMasterBuildVersion}",m_zDevLib)
                kitDisplayName = "${kitName} ${releaseName}"
                kitGav.name =  releaseName
                ///Create a new kit if KitVersion is not set to use existing version
                zLib.logger("Kit required => ${releaseName}-${kitVersion}")
                if (kitVersion == zMasterBuildVersion && kitVersion.toString() != "latest") {
                    Map installBuilderParams = [devRootPath     : zDevRoot + '\\zInstallBuilder',
                                                serviceList     : 'GetKBList4WFOPackage',
                                                subsystem       : subSystems,
                                                buildNumber     : zMasterBuildVersion,
                                                targetRelease   : releaseName,
                                                utilsBranch     : zLibBranch,
                                                bundleParams : [targetRelease: releaseName, resolveRepo: resolveRepo, targetRepo: sourceRepo, groupId: groupId, createBundle: true, bundleName: releaseName, bundleDisplayName: kitDisplayName] as HashMap]

                    println "running m_zInstallBuilder"
                    zI_Result = m_zInstallBuilder { args = installBuilderParams }
                    kitVersion = zMasterBuildVersion
                    kbList = zI_Result."zGetKBList4WFOPackage_KBNumbers".toString()
                    kbList = filterNewKBs(kbList, releaseName)
                    println "FilteredKB: ${kbList}"
                    throwExceptionIfError(zI_Result.errorCode, currentStage)
                    zI_Result = null
                }

        }
        }
    }
}
        stage("Get Eaas") {
            steps {
                echo "Get Eaas: "
                //TODO: uncomment it out to get env provision
                //Block - Start
                zVcdProxy (m_zDevOpsHelper, "Get Eaas")

                echo "Ouput from provisioning : ${m_zDevOpsHelper.getConfigData()."Get Eaas".eaasProp} "
                script {
                    buildParams.DesktopNode = m_zDevOpsHelper.getConfigData()."Get Eaas".eaasProp.Desktop.VMName
                }

                echo "Ouput from provisioning : buildParams.DesktopLabel = ${buildParams.DesktopLabel} "
                //Block - End
                //bat "exit 1"
            }
        }

        stage("Deployment") {
            agent {
                node {
                    label "${buildParams.DesktopLabel}"
                    customWorkspace "${buildParams.DevRoot}"
                }
            }
            steps {
                echo "hello, this is from Deployment "

                //TODO: uncomment the below to have actual deployment going
                script {
                    m_zLogger.logger("*********THIS IS HOW TO REFERENCE TO OUTPUT FROM PREVIOUS STEP *********: \n" +
                            "Code: Get Consolidate IP: m_zDevOpsHelper.getConfigData().\"Get Eaas\".eaasProp.Consolidate.ExternalIP \n" +
                            "Result: Get Consolidate IP: "+ m_zDevOpsHelper.getConfigData()."Get Eaas".eaasProp.Consolidate.ExternalIP)

                    m_zLogger.logger("**********Node to run the deployment and test:" +  m_zDevOpsHelper.getConfigData()."Get Eaas".eaasProp.Desktop.VMName)
                }

                zDeployment (m_zDevOpsHelper, "Deployment")

            }
        }

        stage("Before Gate 2") {
            agent {
                node {
                    label "${buildParams.DesktopLabel}"
                    customWorkspace "${buildParams.DevRoot}"
                }
            }
            steps {

                zSystemTests (m_zDevOpsHelper, "Before Gate 2")
                script {
                    //check the result, special because we do want to update the return value html out so we can publish result
                    def String stepResult = m_zDevOpsHelper.getConfigData()."Before Gate 2".result
                    def String stepResultMessage = m_zDevOpsHelper.getConfigData()."Before Gate 2".errorMessage
                    if (stepResult == "FAILURE") {
                        m_zLogger.logger("*FAILURE:  ${stepResultMessage}")
                        throw new Exception(resultMap.errorMessage)
                    }
                }
                //bat "exit 1"
            }
        }

        stage("Gate 2") {
            agent {
                node {
                    label "${buildParams.DesktopLabel}"
                    customWorkspace "${buildParams.DevRoot}"
                }
            }
            steps {
                zSystemTests (m_zDevOpsHelper, "Gate 2")

                script {
                    //check the result, special because we do want to update the return value html out so we can publish result
                    def String stepResult = m_zDevOpsHelper.getConfigData()."Gate 2".result
                    def String stepResultMessage = m_zDevOpsHelper.getConfigData()."Gate 2".errorMessage
                    if (stepResult == "FAILURE") {
                        m_zLogger.logger("*FAILURE:  ${stepResultMessage}")
                        throw new Exception(stepResultMessage)
                    }
                }
                //bat "exit 1"
            }
        }
        stage("After Gate 2") {
            agent {
                node {
                    label "${buildParams.DesktopLabel}"
                    customWorkspace "${buildParams.DevRoot}"
                }
            }
            steps {
                echo "After Gate 2 - Currently doing nothing "
//                zSystemTests (m_zDevOpsHelper, "After Gate 2")
//                script {
//                    //check the result, special because we do want to update the return value html out so we can publish result
//                    def String stepResult = m_zDevOpsHelper.getConfigData()."After Gate 2".result
//                    def String stepResultMessage = m_zDevOpsHelper.getConfigData()."After Gate 2".errorMessage
//                    if (stepResult == "FAILURE") {
//                        m_zLogger.logger("*FAILURE:  ${stepResultMessage}")
//                        throw new Exception(stepResultMessage)
//                    }
//                }
                //bat "exit 1"
            }
        }

        stage("Promotion") {
            agent {
                node {
                    label "${buildParams.DesktopLabel}"
                    customWorkspace "${buildParams.DevRoot}"
                }
            }
            steps {
                echo "hello, this is from Promotion"
                zArchiveMng (m_zDevOpsHelper, "Promotion")
//                bat "exit 1"
            }
        }

        stage("Change Vapp LeaseTime") {
            steps {
                echo "hello, this is from Change Vapp LeaseTime"
//                bat "exit 1"
            }
        }

    }
    post {
        always {
            echo "I HAVE FINISHED"
            echo "FYI: ConfigData: \n ${m_zDevOpsHelper.getConfigData()}"
        }
        success {
            echo "MOST DEFINITELY FINISHED"
        }
        failure {
            echo "I FAILED"
        }
    }
}



