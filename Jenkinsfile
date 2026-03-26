properties([pipelineTriggers([githubPush()])])

node() {

    stage('Checkout') {
        git url: 'https://github.com/bmajczak/WebApp.git', branch: 'postgres-docker'
    }

    stage('Build') {
        dir('WebApp') {
            sh 'dotnet restore'
            sh 'dotnet build -c Release'
        }
    }

    stage('Test') {
        dir('WebApp') {
            sh 'dotnet test ./Tests/Tests.csproj --configuration Release'
        }
    }

    stage('Publish') {
        dir('WebApp') {
            sh 'dotnet publish WebApp.csproj -c Release -o ./publish'
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
