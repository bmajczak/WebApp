node() {
    stage(name: "Build") {
        sh(script: 'rm -rf WebApp')
        sh(script: 'git clone https://github.com/bmajczak/WebApp.git')
        dir(path: 'WebApp/WebApp') {
            sh(script: 'dotnet add package Microsoft.EntityFrameworkCore.SqlServer')
            sh(script: 'dotnet add package Microsoft.EntityFrameworkCore.Tools')
            
            def isDotNetEfInstalled = sh(script: 'dotnet tool list -g | grep dotnet-ef', returnStatus: true)
            if (isDotNetEfInstalled != 0) {
                sh(script: 'dotnet tool install --global dotnet-ef')
            }

            // Add dotnet-ef to PATH
            env.PATH = "/var/lib/jenkins/.dotnet/tools:${env.PATH}"

            // Generate a unique migration name
            def currentDate = new Date().format("yyyyMMdd_HHmm")
            def migrationBaseName = "Migration"
            def migrationName = "${migrationBaseName}_${currentDate}"

            // Create migration and update the database
            sh(script: "dotnet ef migrations add ${migrationName}")
            sh(script: 'dotnet ef database update')

            // Publish the application
            sh(script: 'dotnet publish -c Release -o ./publish')
        }
    }

    stage(name: 'Deployment') {
        dir(path: 'WebApp/WebApp') {
            sshagent(credentials: ['app01-key']) {
                sh(script: 'ssh -o StrictHostKeyChecking=no vagrant@app01 "sudo systemctl stop webapp.service"')
                sh(script: 'ssh -o StrictHostKeyChecking=no vagrant@app01 "sudo rm -rf /var/www/app/*"')
                sh(script: 'scp -o StrictHostKeyChecking=no -r ./publish/* vagrant@app01:/var/www/app/')
                sh(script: 'ssh -o StrictHostKeyChecking=no vagrant@app01 "sudo systemctl start webapp.service"')
            }
        }
    }
}
