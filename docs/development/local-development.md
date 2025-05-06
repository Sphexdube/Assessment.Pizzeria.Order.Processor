# Local Development

The below breaks down how to develop on this solution. This will aid in setting up the environment to allow for easy development

## Tools

1. <b>Visual Studio</b> - Development of the software using c#
2. <b>Visual Studio Code</b> - Allowing easy editing of documentation etc
3. <b>Docker</b> - Used for docker compose for local development
4. <b>Rancher Desktop</b> - Management of containers
5. <b>Nodejs</b>
6. <b>Mermaid CLI</b>
    Used to generate PNGs from .mermaid files and referenced in docs
    ```cmd
        npm install -g @mermaid-js/mermaid-cli
    ```
7. <b>Vault CLI</b>
    Used to manage the local containerized instance of hashicorp vault
    https://developer.hashicorp.com/vault/install

    * Download for windows
    * Extract zip folder
    * Copy Vault.exe to C:\Windows\system32

## Building
As part of the build process, there is a post build event that is uses the above mentioned mermaid cli to generate png images from the .mermaid files. This is to ensure that images are available in the repository.

### Docker Compose
In order to speed up the development process, instead of depending on external hashicorp instances or sql servers, we use the docker-compose.yaml file (located in [source location]\src\docker-compose.yaml) to spin up local containers that will build a few things,

1. HashiCorp Vault instance
2. Sql Server instance
3. Deploy Sql Server state

The above speeds up the process of developing software dramatically as developers do not have to fall over each others feet.

Before you start your solution, you would need to run the following command,
```cmd
    docker compose up -d
```

### Docker
In order to allow better containerization, we have to think about using docker on our local machines. The Dockerfile can be found under [checkout location]\build\docker\Dockerfile

If you want to test the Dockerfile locally, you will first need to publish the current solution, the best way to do that is,
* Navigate to the base folder i.e. so that you can see the respective src and build folders
* Execute the following command,
```cmd
dotnet publish Order.Presentation.Api\Order.Presentation.Api.csproj -c Release -o ..\publish -r linux-x64 --self-contained --no-restore /p:DebugType=none /p:PublishSingleFile=true
```

The above will generate a publish folder that contains the published file that will be used in the Dockerfile. To now create the docker image, run the following command,
```cmd
docker build --no-cache -f build/docker/Dockerfile -t order .
```

The above will create a docker image and add it to your local docker image registry. Now you are able to run the local instance of this docker image by executing the following,
```cmd
docker run -p 5000:5000 order
```

The above will expose the docker image on the local machine on the url http://localhost:5000

### Database
As part of the docker-compose file we execute the sql files that are found in the following shared projects,
1. dbOrder
2. dbOrderArchive

These above mentioned projects contain the sql files and using a tool called [Grate](https://erikbra.github.io/grate/) we deploy the sql files into a sql server instance using a migration based deployment.

<i>**NOTE:** Entity framework is not defining the state of the database, the sql fiiles are.</i>

### HashiCorp Vault
The Vault instance locally is started up using

Next open up powershell, and navigate to [code location]/build/vault/localhost/init
Within this folder in powershell, run the following command,
```powershell
.\setup.ps1
```

This will take the json file that resides in the [code location]/build/vault/localhost/data location and add it as an appsettings entry in the secrets key vault.
The output of the script will be a RoleId and a SecretId, this will be your 2 keys to add to the Localhost config for debug purposes.

### Metrics
To view the metrics values produced as part of the code (Custom metrics), you can run the following command
```cmd
dotnet-counters monitor -n "Order.Presentation.Api" --counters "Tfg.StoreSytems.Order"
```

If the tool is not installed, you can install it with the following command,
```cmd
dotnet tool update -g dotnet-counters
```

NOTE: With the above commands ensure you are running in Administrator mode.

## Branching

The solution follows a GitFlow branching strategy. We use a tool called [GitVersion](https://gitversion.net/docs/) to aid in version determination for building solutions.
The versioning is used out of the box, however a better understanding of GitFlow can be found [here](https://gitversion.net/docs/learn/branching-strategies/gitflow/examples).

When working on new features,
### Branch off development
1. Create a new branch in the format of feature/<feature being added> off development branch
2. Make changes to feature branch
3. Submit Pull request to merge into development branch
4. Wait for signoff and merge to be completed
5. Development branch will kick off
6. Solution will be deployed to dev

Once in the development branch, we wait to take the solution to production, once ready,
1. Merge into main branch
2. Test deployment will automatically kick off
3. Solution will be deployed to test

Once in the main branch, and when signoff for prod deployment is obtained, then
1. Tag the main branch with the required version (The version of the Test build)
2. Tag build will be kicked off
3. Solution will be deployed to prod

When working on hotfixes
### Branch off main
1. Create a new branch in the format of hotfix/<hotfix being added> off main branch
2. Make changes to hotfix branch
3. Submit Pull request to merge into main branch
4. Wait for signoff and merge to be completed
5. Main branch will kick off
6. Solution will be deployed to Test

Once in the main branch, we wait to take the solution to production, once ready,
1. Tag the main branch with the required version (The version of the Test build)
2. Tag build will be kicked off
3. Solution will be deployed to prod

Now we have to take the changes back to development,
1. Merge main down to development
2. Development branch will kick off
3. Solution will be deployed to dev

![GitFlow](https://gitversion.net/docs/img/DocumentationSamplesForGitFlow_FeatureFromDevelopBranch.png)