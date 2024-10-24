pipeline{
    agent{
        label "node"
    }
    environment {
        // 使用 Jenkins 凭据管理存储敏感信息
        DOCKER_CREDENTIALS_ID = 'docker-credentials-id' // 替换为您的 Jenkins 凭据 ID
        IMAGE_NAME = 'XStudio' // 替换为您的镜像名称
        REGISTRY_URL = '172.30.103.227:5000' // Docker 仓库地址
        GIT_URL = 'https://github.com/sclshu3714/X-Studio.git' // GitLab 仓库地址
        GIT_BRANCH = 'main' // GitLab 仓库分支
        K8S_DEPLOYMENT_FILE = 'deployment.yaml' // Kubernetes 部署文件名称
        TAG = "${env.BUILD_NUMBER}-${env.BRANCH_NAME}" // 使用构建号和分支名生成标签
    }
    stages{
        stage("拉取代码"){
            steps{
                echo "========executing 拉取代码========"
                git url: GIT_URL, branch: GIT_BRANCH // 替换为您的 GitLab 仓库 URL 和分支
            }
            post{
                always{
                    echo "========always========"
                }
                success{
                    echo "========拉取代码 executed successfully========"
                }
                failure{
                    echo "========拉取代码 execution failed========"
                }
            }
        },
        stage("构建项目"){
            steps{
                echo "========executing 构建项目========"
                // 进入项目目录（如果需要）
                dir('your-project-directory') { // 替换为您的项目目录
                    // 执行构建命令
                    sh 'dotnet build' // 对于 Windows，可以使用 'bat' 命令
                }
            }
            post{
                always{
                    echo "========always========"
                }
                success{
                    echo "========构建项目 executed successfully========"
                }
                failure{
                    echo "========构建项目 execution failed========"
                }
            }
        },
        stage("测试项目"){
            steps{
                echo "========executing 测试项目========"
                // 进入项目目录（如果需要）
                dir('your-project-directory') { // 替换为您的项目目录
                    // 执行测试命令
                    sh 'dotnet test' // 对于 Windows，可以使用 'bat' 命令
                }
            }
            post{
                always{
                    echo "========always========"
                }
                success{
                    echo "========测试项目 executed successfully========"
                }
                failure{
                    echo "========测试项目 execution failed========"
                }
            }
        },
        stage("构建镜像"){
            steps{
                echo "========executing 构建镜像========"
                script {
                    // 进入 Dockerfile 所在的目录（如果需要）
                    dir('your-dockerfile-directory') { // 替换为您的 Dockerfile 目录
                        // 构建 Docker 镜像
                        sh 'docker build -t {IMAGE_NAME}:{TAG} .' // 替换为您的镜像名称和标签
                    }
                }
            }
            post{
                always{
                    echo "========always========"
                }
                success{
                    echo "========构建镜像 executed successfully========"
                }
                failure{
                    echo "========构建镜像 execution failed========"
                }
            }
        },
        stage("推送镜像"){
            steps{
                echo "========executing 推送镜像========"
                script {
                    // 登录到 Docker 仓库（如果需要）
                    sh 'docker login -u your-username -p your-password your-registry-url' // 替换为您的用户名、密码和仓库 URL
                    
                    // 推送 Docker 镜像
                    sh 'docker push your-image-name:your-tag' // 替换为您的镜像名称和标签
                }
            }
            post{
                always{
                    echo "========always========"
                }
                success{
                    echo "========推送镜像 executed successfully========"
                }
                failure{
                    echo "========推送镜像 execution failed========"
                }
            }
        },
        stage("发版项目"){
            steps{
                echo "========executing 发版项目========"
                script {
                    // 进入发版目录（如果需要）
                    dir('your-deployment-directory') { // 替换为您的发版目录
                        // 部署到 Kubernetes 的命令 (根据实际需要修改)
                        sh 'kubectl apply -f deployment.yaml' // 替换为您的部署文件
                    }
                }
            }
            post{
                always{
                    echo "========always========"
                }
                success{
                    echo "========发版项目 executed successfully========"
                }
                failure{
                    echo "========发版项目 execution failed========"
                }
            }   
        }
    }
    post{
        always{
            echo "========always========"
        }
        success{
            echo "========pipeline executed successfully ========"
        }
        failure{
            echo "========pipeline execution failed========"
        }
    }
}