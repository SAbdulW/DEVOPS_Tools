import hudson.FilePath
import jenkins.model.Jenkins
import hudson.model.Node
import hudson.model.Computer

def sTempLabel = "swarm Depreciation-Slaves"
def build = Thread.currentThread().executable
def String sNodeName = build.getEnvironment(listener).get('MachineName')
Jenkins jenkins = Jenkins.instance
for (Node node in jenkins.nodes) {
  def Computer comp = node.toComputer()
  def String sDisplayName = comp.getDisplayName()
  if (sDisplayName ==~ "${sNodeName}") {
    println "Found node: ${sDisplayName}"
    if ((node.getLabelString() != sTempLabel)) {
      //node.setLabelString(sTempLabel)
      println "Node: ${sNodeName} has moved to pool: " + sTempLabel
    }
    break
  }
}