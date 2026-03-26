properties([pipelineTriggers([githubPush()])])

node() {

    stage('Checkout') {
        git url: 'https://github.com/bmajczak/WebApp.git', branch: 'postgres-utc'
    }

    stage('Publish') {
        dir('WebApp/WebApp') {
            sh 'dotnet publish -c Release -o ./publish'
        }
    }

    stage('Test') {
        dir('WebApp') {
            sh(script: 'dotnet build ./Tests/Tests.csproj --configuration Release')
            sh(script: 'dotnet test ./Tests/Tests.csproj --no-build --configuration Release --verbosity normal')
        }
    }

    stage('Deploy') {
        dir('WebApp/WebApp') {
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
