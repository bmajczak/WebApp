properties([pipelineTriggers([githubPush()])])

node() {

    stage('Checkout') {
        deleteDir()
        git url: 'https://github.com/bmajczak/WebApp.git', branch: 'postgres-docker'
    }

    stage('Publish') {
        dir('WebApp') {
            sh 'dotnet publish WebApp.csproj -c Release -o ./publish'
        }
    }
    
    stage('Test') {
        dir('WebApp') {
            sh(script: 'dotnet build ./Tests/Tests.csproj --configuration Release')
            sh(script: 'dotnet test ./Tests/Tests.csproj --no-build --configuration Release --verbosity normal')
        }
    }

    withEnv(['DOCKER_IMAGE=""']){
        stage('Docker Build') {
            DOCKER_IMAGE = docker.build("bmajczak/webapp-postgres:${env.BUILD_NUMBER}")
        }

        stage('Docker Push') {
            docker.withRegistry('', 'docker-creds') {
                DOCKER_IMAGE.push()
                DOCKER_IMAGE.push('latest')
            }
        }
    }

    stage('Deploy') {
        withKubeConfig([credentialsId: 'kubeconfig-creds']) {
            dir('k8s/overlays/prod') {
                sh(script: "kustomize edit set image bmajczak/webapp-postgres=bmajczak/webapp-postgres:${env.BUILD_NUMBER}")
                sh(script: 'kubectl apply -k .')
            }
        }
    }
}
