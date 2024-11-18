node() {
    stage(name: "Build") {
        sh(script: 'rm -rf WebApp')
        sh(script: 'git clone https://github.com/bmajczak/WebApp.git')
        dir(path: 'WebApp/WebApp') {
            sh(script: 'dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.8')
            sh(script: 'dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.8')
            sh(script: 'dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.8')
            sh(script: 'dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.8')
            sh(script: 'dotnet add package xunit')
            sh(script: 'dotnet add Tests package xunit --version 2.9.2')
            sh(script: 'dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 8.0.8')
            sh(script: 'dotnet add package Microsoft.AspNetCore.Mvc.Testing --version 8.0.8')
            sh(script: 'dotnet add package xunit.runner.visualstudio --version 2.8.2')

            def isDotNetEfInstalled = sh(script: 'dotnet tool list -g | grep dotnet-ef', returnStatus: true)
            if (isDotNetEfInstalled != 0) {
                sh(script: 'dotnet tool install --global dotnet-ef')
            }

            env.PATH = "/var/lib/jenkins/.dotnet/tools:${env.PATH}"

            def currentDate = new Date().format("yyyyMMdd_HHmm")
            def migrationBaseName = "Migration"
            def migrationName = "${migrationBaseName}_${currentDate}"

            sh(script: "dotnet ef migrations add ${migrationName}")
            sh(script: 'dotnet ef database update')

            sh(script: 'dotnet publish -c Release -o ./publish')
        }
    }

    stage(name: "Test") {
        dir(path: 'WebApp/WebApp') {
            sh(script: 'dotnet test ./Tests/Tests.csproj --no-build --verbosity normal')
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
