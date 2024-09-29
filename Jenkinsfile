node() {
    stage(name: "Build"){
        sh(script: 'rm -rf WebApp')
        sh(script: 'git clone https://github.com/bmajczak/WebApp.git')
        dir(path: 'WebApp/WebApp'){
            sh(script: 'dotnet publish')
        }
    }
    stage(name: 'Deployment'){
        dir(path: 'WebApp/WebApp'){
        sh(script: 'ssh -o StrictHostKeyChecking=no vagrant@app01 "sudo systemctl stop webapp.service')
        sh(script: 'ssh -o StrictHostKeyChecking=no vagrant@app01 "sudo rm -rf /var/www/app/*')
        sh(script: 'scp -o StrictHostKeyChecking=no ./bin/Release/net8.0/publish/* vagrant@app01:/var/www/app/')
        sh(script: 'ssh -o StrictHostKeyChecking=no vagrant@app01 "sudo systemctl start webapp.service')
        }
    }
}