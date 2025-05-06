# Deployment

Deployment of the solution is handled by the cicd-pipeline.yaml file found in [code location]\build\pipelines\cicd-pipeline.yaml. This file is used by Azure Devops to create the CICD pipeline that will be used for deployment.

## Triggered builds
Builds are automatically triggered on the following branches
* development
* main
* repository tags

### development
These builds will deploy changes to the development environment, in this case the Development OpenShift Cluster.

### main
These builds will deploy changes to the test environment, in this case the Test OpenShift Cluster.

### repository tags
Once you are ready to deploy your solution into the production environment, you need to tag your repository with the respective version (the same version number used for test builds).
This will then deploy the image that was used in the test environment into the production environment.

## OpenShift details
namespaces: