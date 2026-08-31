pipeline {
    agent any

    environment {
        AWS_REGION = 'eu-central-1'
        ECR_REGISTRY = '332428815411.dkr.ecr.eu-central-1.amazonaws.com'
        ECR_REPOSITORY = 'devsecops/webapp'
        EKS_CLUSTER = 'eks_cluster'
        K8S_NAMESPACE = 'devsecops'
        DEPLOYMENT_NAME = 'app'
        CONTAINER_NAME = 'app'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build image') {
            steps {
                sh '''
                    docker build \
                      -t ${ECR_REGISTRY}/${ECR_REPOSITORY}:${BUILD_NUMBER} .
                '''
            }
        }

        stage('Login to ECR') {
            steps {
                sh '''
                    aws ecr get-login-password \
                      --region ${AWS_REGION} \
                    | docker login \
                      --username AWS \
                      --password-stdin \
                      ${ECR_REGISTRY}
                '''
            }
        }

        stage('Push image') {
            steps {
                sh '''
                    docker push \
                      ${ECR_REGISTRY}/${ECR_REPOSITORY}:${BUILD_NUMBER}
                '''
            }
        }

        stage('Deploy to EKS') {
            steps {
                sh '''
                    kubectl set image \
                      deployment/${DEPLOYMENT_NAME} \
                      ${CONTAINER_NAME}=${ECR_REGISTRY}/${ECR_REPOSITORY}:${BUILD_NUMBER} \
                      -n ${K8S_NAMESPACE}
                '''
            }
        }

        stage('Wait for rollout') {
            steps {
                sh '''
                    kubectl rollout status \
                      deployment/${DEPLOYMENT_NAME} \
                      -n ${K8S_NAMESPACE} \
                      --timeout=120s
                '''
            }
        }
    }
}
