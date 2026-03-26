properties([pipelineTriggers([githubPush()])])

node() {

    stage('Checkout') {
        git url: 'https://github.com/bmajczak/WebApp.git', branch: 'postgres-utc'
    }

    stage('Build') {
        dir('WebApp') {

            // Pakiety (PostgreSQL + EF Core 8.0.8)
            sh 'dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.8'
            sh 'dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.8'
            sh 'dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.8'
            sh 'dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.8'

            // Build
            sh 'dotnet restore'
            sh 'dotnet build -c Release'
        }
    }

    stage('Test') {
        dir('WebApp') {
            sh 'dotnet test ./Tests/Tests.csproj --configuration Release'
        }
    }

    stage('Database Migration') {
        dir('WebApp') {

            // upewnij się że dotnet-ef istnieje
            def isInstalled = sh(script: 'dotnet tool list -g | grep dotnet-ef', returnStatus: true)
            if (isInstalled != 0) {
                sh 'dotnet tool install --global dotnet-ef --version 8.0.8'
            }

            env.PATH = "/var/lib/jenkins/.dotnet/tools:${env.PATH}"

            sh 'dotnet ef database update'
        }
    }

    stage('Publish') {
        dir('WebApp') {
            sh 'dotnet publish -c Release -o ./publish'
        }
    }

    stage('Deploy') {
        dir('WebApp') {
            sshagent(credentials: ['app01-key', 'app02-key']) {

                def servers = ['app01', 'app02']

                for (server in servers) {
                    sh "ssh -o StrictHostKeyChecking=no vagrant@${server} \"sudo systemctl stop webapp.service\""
                    sh "ssh -o StrictHostKeyChecking=no vagrant@${server} \"sudo rm -rf /var/www/app/*\""
                    sh "scp -o StrictHostKeyChecking=no -r ./publish/* vagrant@${server}:/var/www/app/"
                    sh "ssh -o StrictHostKeyChecking=no vagrant@${server} \"sudo systemctl start webapp.service\""
                }
            }
        }
    }
}
