properties([pipelineTriggers([githubPush()])])

node() {
    stage('Checkout') {
        git url: 'https://github.com/bmajczak/WebApp.git', branch: 'main'
    }

    stage('Build') {
        dir('WebApp/WebApp') {
            // Pakiety EF Core 8.0.8 dla SQL Server
            sh 'dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.8'
            sh 'dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.8'
            sh 'dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.8'
            sh 'dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.8'
            sh 'dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 8.0.8'
            sh 'dotnet add package Microsoft.AspNetCore.Mvc.Testing --version 8.0.8'
            
            // Testy
            sh 'dotnet add Tests package xunit --version 2.9.2'
            sh 'dotnet add Tests package xunit.runner.visualstudio --version 2.8.2'

            // dotnet-ef
            def isDotNetEfInstalled = sh(script: 'dotnet tool list -g | grep dotnet-ef', returnStatus: true)
            if (isDotNetEfInstalled != 0) {
                sh 'dotnet tool install --global dotnet-ef --version 8.0.8'
            }

            env.PATH = "/var/lib/jenkins/.dotnet/tools:${env.PATH}"

            sh 'dotnet ef database update'

            sh 'dotnet publish -c Release -o ./publish'
        }
    }

    stage('Test') {
        dir('WebApp/WebApp') {
            sh 'dotnet build ./Tests/Tests.csproj --configuration Release'
            sh 'dotnet test ./Tests/Tests.csproj --no-build --configuration Release --verbosity normal'
        }
    }

    stage('Deploy') {
        dir('WebApp/WebApp') {
            sshagent(credentials: ['app01-key', 'app02-key']) {
                def servers = ['app01', 'app02']
                
                for (server in servers) {
                    echo "Deploying to ${server}..."
                    sh "ssh -o StrictHostKeyChecking=no vagrant@${server} 'sudo systemctl stop webapp.service'"
                    sh "ssh -o StrictHostKeyChecking=no vagrant@${server} 'sudo rm -rf /var/www/app/*'"
                    sh "scp -o StrictHostKeyChecking=no -r ./publish/* vagrant@${server}:/var/www/app/"
                    sh "ssh -o StrictHostKeyChecking=no vagrant@${server} 'sudo systemctl start webapp.service'"
                }
            }
        }
    }
}
