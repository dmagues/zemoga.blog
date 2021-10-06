
## Welcome to Zemoga Blog

The blog consist of 2 projects:

- **zemoga.blog.api**: Rest api project
- **zemoga.blog.webui**: Front end

## Installation

**Api Project**

In dev env, the api runs on http://localhost/5001, and the first time it runs, it creates the database (sqllite) with an admin user:

usename: **admin**
password: **Asdfgh654321**

if you are running from idle (VS 2019 or above) you can access the swagger doc:

http://localhost:5001/swagger

All functionality describe in the specifications have been code regardless any bug the API could have.


**Web UI Project**

You have to set **RestApiUrl** parameter in the "appsettings.json". Currently, it has set in appsetting.Development.json: 

    "AppSettings": {
	    "RestApiUrl": "http://localhost:5001/api/"
    }

Through the UI, you can register new users and login. Create, edit, approve, reject posts. It is pending to create comments on the post.




