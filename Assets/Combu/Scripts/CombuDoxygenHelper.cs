/*
 * IGNORE THIS FILE.
 * This file is only here for internal usage, specifically it contains special commands for Doxygen to automatically generate the API documentation.
 * You can ignore it, anyway consider that you can run Doxygen (http://www.doxygen.org) and specify this folder as source to create the documentation directly from the scripts.
 */

/*! \mainpage API Reference
    \section sec_main_intro Introduction
	<a href="https://www.skaredcreations.com/wp/products/combu/" target="_blank">Combu</a> is a full featured solution
	to add online storage management for your player login system, highscores, friends, inventory and more
	for your games using a web server and MySQL database. It is shipped with client source code for Unity3D.

	This documentation is for <strong>version 3.x</strong> only. The documentation for <strong>version 2.x</strong> is located at <a href="https://www.skaredcreations.com/api/combu/v2">here</a>.
	
	The 3rd generation of <strong>Combu</strong> features huge improvement to security: now every call from client to server is encrypted through AES keys generated on the fly at the beginning of every launch and exchanged with the server with RSA key (also generated on the fly for each new connection).
	
	\section sec_main_install Installation
	<ol>
		<li>
			<strong>Purchase Combu</strong>
			<br/>You can purchase Combu from <strong>Skared Creations</strong> <a href="https://www.skaredcreations.com/wp/downloads/combu/" target="_blank">website</a>
			or Unity3D <a href="http://u3d.as/4tg" target="_blank">Asset Store</a>.
		</li>
		<li>
			<strong>Import Combu package</strong>
			<br/>Create an empty scene and import the Combu unitypackage (or from Asset Store window).
		</li>
		<li>
			<strong>Setup the web server</strong>
			<br/><a href="page_server.html">Click here</a> to see how to configure your web server, you can use a local web server before going live to production for example <a href="https://www.apachefriends.org" target="_blank">XAMPP</a> and extract the file <strong>combu_web.zip</strong> (you find it in the folder <em> Install</em> of Combu in Unity) in the root of the web server (or in any other folder of your choice inside the web server tree).
		</li>
		<li>
			<strong>Navigate to /_setup</strong>
			<br/>You can use your web browser to navigate to  http://yourserver/combu_folder_path/_setup to generate the content for your configuration file (located at <em>/lib/config.php</em>) and the SQL queries to create the tables on your database.
		</li>
		<li>
			<strong>Configure Combu Manager</strong>
			<br/>Drag the prefab <em>Combu/Prefabs/CombuManager</em> in the scene and set the correct settings in the <strong>CombuManager</strong> component inspector in order to connect to your web server. To get started you can just set the properties <strong>Url Root Production</strong> to the URL of your live production environment and <strong>Url Root Stage</strong> to the URL of your development environment (usually your local machine),
			then select the checkbox <strong>Use Stage</strong> to run the development environment or leave it unchecked to run the live production.
		</li>
	</ol>
	The first time you will access the admin console (at: http://yourserver/combu_folder_path/admin) the system will create
	a new admin account with username and password <strong><em>admin</em></strong> and you should be automatically logged. As soon as you access
	the admin console for the first time on your live server you're strongly suggested to change the password of user <strong>admin</strong>
	in the section <strong>Admins</strong> or delete it and create a new user with a different username and password.

	To start now please visit the welcome page <a href="https://www.skaredcreations.com/wp/products/combu/getting-started/" target="_blank">Getting started</a> or go through the <a href="pages.html">Related Pages</a> and <a href="annotated.html">API reference</a> in this documentation.

	\section sec_main_doc Documentation
	You can download the API documentation as offline <a href="refman.pdf" target="_blank">PDF</a>
	or navigate online at <a href="https://www.skaredcreations.com/api/combu">https://www.skaredcreations.com/api/combu</a>

	You can also use <a href="http://www.doxygen.org" target="_blank">Doxygen</a> to build the HTML/PDF documentation directly from the Unity scripts of Combu.

	\section sec_main_assetstore Customers from Asset Store
	If you purchased Combu on <strong>Unity Asset Store</strong>, now you can redeem the download on this website too at no cost by providing the Invoice No.
	<a href="https://www.skaredcreations.com/wp/redeem-asset-store-invoice/" target="_blank">Click here</a> to redeem your invoice now.
 */

/*! \page page_doc Off-line documentation
	\tableofcontents
	\brief Download this documentation as <a href="refman.pdf" target="_blank">PDF</a>
 */

/*! \page page_server Web server setup
	\tableofcontents
	\brief In this section you will learn how to setup a web server on your local machine and install Combu.
	
	\section sec_server_production Live server
	<strong>Combu</strong> will work correctly in almost every hosting provider, since the production servers are usually configured correctly.
	So in production environment you only need to upload all files including the folder <em>_setup</em>, navigate with your browser to http://yourserver/yourcombu/_setup
	and complete both the configuration file section and the database tables creation.
	
	You can skip the configuration file section if you already configured it locally, in this case you will edit <em>lib/config.php</em> for the database connection and root URL defines
	(<em>URL_ROOT</em>, <em>GAME_DB_SERVER</em>, <em>GAME_DB_NAME</em>, <em>GAME_DB_USER</em> and <em>GAME_DB_PASS</em>), as explained in the <a href="http://youtu.be/PsZyYopzi40" target="_blank">tutorial</a> video.

	After the configuration file has been eventually edited and the database tables are created, you should delete the folder <em>_setup</em> from your production server.

	\section sec_server_local Local machine
	Before using a live server you may want to test your game earlier and work locally on your machine, in this case
	you have few alternatives (Microsoft IIS, Apache, XAMPP, etc) and what's to get only depends on your expertise and skills.
	<ol>
		<li>
			For the sake of this sample we will assume you have installed <a href="https://www.apachefriends.org/" target="_blank"><strong>XAMPP</strong></a>,
			it's a well-known free package that contains both the web server Apache and the MySQL server engine (it exists few similar packages, like LAMPP, they're almost the same for this example)
		</li>
		<li>
			<strong>MySQL</strong> must be configured with case-sensitivity for table names, it's usually already configured on live servers by hosting providers;
			if you're running on your local <strong>Windows</strong> machine then edit the file <strong>my.ini</strong> (you'll find it in one of the folders inside XAMPP installation)
			and add a new line with <strong>lower_case_table_names=2</strong> just below the line that contains <strong>[mysqld]</strong>
			(if you haven't the section <em>[mysqld]</em> then add both lines at the end of the file)
		</li>
		<li>
			Start both HTTP and MySQL services (if you're using XAMPP, it can be done through the XAMPP Control Panel else from Windows Services)
		</li>
		<li>
			Create an empty database with name <strong>combu</strong> (or any other name of your choice) in phpMyAdmin
			(in XAMPP it's usually installed at <em>http://localhost/phpmyadmin</em>) or <a href="http://www.mysql.com/products/workbench/" target="_blank">MySQL Workbench</a>
		</li>
		<li>
			Uncompress the file <strong>combu_web.zip</strong> into your local web server root (if you're using XAMPP then it's usually located in the folder <em>xamppfiles/htdocs</em>)
			and use your web browser to navigate to http://yourserver/combu_folder_path/_setup (for example http://localhost/combu/_setup) to generate the content for your configuration file
			(located at <em>/lib/config.php</em>) and the SQL queries to create the tables on your database.
		</li>
	</ol>

	\section sec_server_config Server and Client configuration
	Now that you learned how to setup your web server and have installed <strong>Combu</strong> server, you can configure the system at your needs.
	
	Continue to \ref page_config.
*/

/*! \page page_config Server and Client configuration
							\tableofcontents
							\brief In this section you will learn how to setup your server and client.

							\section sec_config_server Server configuration
							We really suggest to navigate to http://yourserver/combu_folder_path/_setup of your development environment to get the content for your configuration file,
							because it's mantained up to date with all the settings that can be configured with detailed descriptions.

							The configuration file is located at <strong>lib/config.php</strong> and contains the PHP defines of every configuration setting (if you are new to PHP, "define" is the equivalent of "const" in C#):
							<ul>
							<li>Generic<ul>
							<li><strong>URL_ROOT</strong> (default: /combu/): Define the relative URL of Combu folder, including the last path separator (e.g.: /your_combu_path/)</li>
							<li><strong>WS_DEBUG</strong> (default: FALSE): Debug the web services data: if TRUE then the data is loaded from both GET and POST, else it is loaded only from POST (set to FALSE or comment in the production environment)</li>
							<li><strong>DEFAULT_LIST_LIMIT</strong> (default: 20): Default size of a records list when limit is not specified in the REQUEST (must be greater than zero)</li>
							<li><strong>RANDOM_CODE_CHARS</strong> (default: bcdfghjklmnpqrstvwxzBCDFGHJKLMNPQRSTVWXZ0123456789): Alphanumeric characters used to generate a random code (e.g.: Activation Code and Reset Password Authorization Code)</li>
							<li><strong>RANDOM_CODE_LENGTH</strong> (default: 10): Default length of a random code (e.g.: Activation Code and Reset Password Authorization Code)</li>
							</ul></li>
							<li>Database connection<ul>
							<li><strong>GAME_DB_USE_PDO</strong> (default: TRUE): Should use PDO for the database functions? (If FALSE then it uses mysqli)</li>
							<li><strong>GAME_DB_TYPE</strong> (default: mysql): Database engine (only if PDO is enabled, this must be supported by PDO)</li>
							<li><strong>GAME_DB_SERVER</strong>: Database Server hostname or IP (if the engine is running on the same machine use 'localhost')</li>
							<li><strong>GAME_DB_PORT</strong> (default: 3306): The port of the database connection (default MySql port is 3306)</li>
							<li><strong>GAME_DB_NAME</strong>: The name of the database</li>
							<li><strong>GAME_DB_PREFIX</strong>: Prefix for table names</li>
							<li><strong>GAME_DB_USER</strong>: The username of the database connection</li>
							<li><strong>GAME_DB_PASS</strong>: The password of the database connection</li>
							</ul></li>
							<li>Security<ul>
							<li><strong>RSA_PRIVATE_KEY_BITS</strong> (default: 2048): Key bits length for RSA generation (higher values increase security but require more time to generate keys), can be 1024, 2048 or 4096</li>
							<li><strong>RESPONSE_ENCRYPTED</strong> (default: TRUE): Should encrypt the server response to web services? (TRUE increase security but requires more resources/time both in server and in client)</li>
                            <li><strong>DENY_UNVERSIONED_CLIENT</strong> (default: FALSE): Should refuse the connections when the clients don't send their version number?</li>
                            <li><strong>MIN_CLIENT_VERSION</strong> (default: empty): Minimum version of client's CombuManager.COMBU_VERSION to be considered as out of range</li>
                            <li><strong>MAX_CLIENT_VERSION</strong> (default: empty): Maximum version of client's CombuManager.COMBU_VERSION to be considered as out of range</li>
                            <li><strong>DENY_OUTRANGE_VERSIONS</strong> (default: FALSE): Should refuse the connections with out of range versions?</li>
							</ul></li>
							<li>Email<ul>
							<li><strong>EMAIL_SMTP</strong> (default: FALSE): it allows to send the emails through SMTP server</li>
							<li><strong>EMAIL_SMTP_SECURE</strong> (default: NULL): secure authentication type for the SMTP connection (can be: NULL, 'ssl' or 'tls')</li>
							<li><strong>EMAIL_SMTP_HOSTNAME</strong> (default: NULL): server hostname or IP for the SMTP connection</li>
							<li><strong>EMAIL_SMTP_PORT</strong> (default: NULL): server port for the SMTP connection (e.g. 465)</li>
							<li><strong>EMAIL_SMTP_USERNAME</strong> (default: NULL): username for the SMTP connection</li>
							<li><strong>EMAIL_SMTP_PASSWORD</strong> (default: NULL): password for the SMTP connection</li>
							<li><strong>EMAIL_SENDER_ADDRESS</strong> (default: noreply@yourserver.com): the sender address of outgoing mail</li>
							<li><strong>EMAIL_SENDER_NAME</strong> (default: Your App Title): the sender name of outgoing mail</li>
                            </ul></li>
                            <li>Email<ul>
							<li><strong>NEWSLETTER_SENDER_ADDRESS</strong> (default: noreply@yourserver.com): the sender address of outgoing newsletters</li>
							<li><strong>NEWSLETTER_SENDER_NAME</strong> (default: Your Newsletter): the sender name of outgoing newsletters</li>
                            </ul></li>
                            <li>User Registration<ul>
							<li><strong>REGISTER_EMAIL_REQUIRED</strong> (default: FALSE): it will require a valid email address upon new user creation</li>
							<li><strong>REGISTER_EMAIL_MULTIPLE</strong> (default: FALSE): it allows to use the same email address for multiple accounts</li>
							<li><strong>REGISTER_EMAIL_ACTIVATION</strong> (default: FALSE): it will create an activation code during user creation and sends it by email, then will require to activate the account before being able to login</li>
							<li><strong>REGISTER_EMAIL_SUBJECT</strong> (default: Confirm account registration): the subject text of the user registration email</li>
							<li><strong>REGISTER_EMAIL_MESSAGE</strong> (default: email_register.html): Name of the HTML/text file containing the mail message (the file must exists in /email_templates); the HTML/text file can contain the following special words:
							<ul>
							<li><strong>{ACTIVATION_URL}</strong>: it will be replaced with the URL to activate the account (if <strong>REGISTER_EMAIL_ACTIVATION</strong> is <em>TRUE</em>)</li>
							<li><strong>{USERNAME}</strong>: it will be replaced with the chosen username</li>
							</ul></li>
							<li><strong>REGISTER_EMAIL_HTML</strong>: it establishes if the user registration email is in HTML or text</li>
                            <li><strong>RESETPWD_EMAIL_SUBJECT</strong> (default: Reset your password): the reset-password mail subject</li>
                            <li><strong>RESETPWD_EMAIL_MESSAGE</strong> (default: email_resetpwd.html): Name of the HTML/text file containing the mail message (the file must exists in /email_templates); the HTML/text file can contain the following special words:
                            <ul>
                            <li><strong>{CODE}</strong>: it will be replaced with the code to pass to User.ChangePassword to set a new password (if the user has an e-mail address stored)</li>
                            <li><strong>{USERNAME}</strong>: it will be replaced with the username</li>
                            </ul></li>
							</ul></li>
							<li>Users<ul>
							<li><strong>ONLINE_SECONDS</strong>: time interval in seconds to consider a user online from last action registered</li>
							<li><strong>CLEAR_PLAYER_SESSIONS</strong>: if set to TRUE then every time a player logs in, the system will delete older login sessions to maintain the sessions table cleaned and as smaller as possible</li>
							<li><strong>GUEST_PREFIX</strong>: the prefix string attached to the id for guest accounts (ex.: "Guest-")</li>
							<li><strong>FRIENDS_REQUIRE_ACCEPT</strong> (default: FALSE): if set to TRUE then the friend add action will require the destination user to accept or decline the request before it appears in the friends list</li>
							</ul></li>
							<li>Upload<ul>
							<li><strong>UPLOAD_DELETE_OLD_FILES</strong> (default: TRUE): if TRUE will keep your upload folder clean with latest changes to UserFile by deleting old file upon upload of new content</li>
							</ul></li>
							<li>Log<ul>
							<li><strong>LOG_FILENAME</strong> (default: app.log): Name of the log file (in the folder /_logs)</li>
							<li><strong>LOG_MAXFILESIZE</strong>: it is the maximum size in bytes of the log file</li>
							<li><strong>LOG_BANNED</strong> (default: FALSE): if set to TRUE then the system will add the requests from banned IP address in the log file</li>
							<li><strong>LOG_MAXFILESIZE</strong> (default: FALSE): if set to TRUE then the system will add the unauthorized/invalid requests in the log file</li>
							</ul></li>
							</ul>

							\section sec_config_client Client configuration
							In the inspector of <strong>CombuManager</strong> script (the prefab is just a GameObject with <em>CombuManager</em> script attached) you will find the following properties:
							<ul>
							<li><strong>Dont Destroy On Load</strong> (default: TRUE): if checked, this GameObject will be alive for the whole lifetime of your game/app; if you want to login in one scene (for example main menu) and last until the game quits then you should enable this flag</li>
							<li><strong>Set As Default Social Platform</strong>: since Combu implements the Unity <strong>ISocialPlatform</strong> interface, if you enable this flag the Combu will be set as current platform on Unity and you will be able to use it also through <strong>Social.Active</strong></li>
							<li><strong>Url Root Production</strong>: is the URL to the folder where you installed the web services on your production server (e.g.: <em>http://www.yourserver.com/combu/</em>)</li>
								<li><strong>Url Root Stage</strong>: is the URL to the folder where you installed the web services on your stage/development server, usually local machine (e.g.: <em>http://localhost/combu/</em>)</li>
									<li><strong>Use Stage</strong>: if checked, the web services calls will be directed to the stage server instead of production</li>
									<li><strong>Log Debug Info</strong>: if checked, all the calls to webservices will add the result text in the console log</li>
									<li><strong>Remember Credentials</strong>: if checked, the last successful login action will store the username and password (encrypted) in PlayerPrefs to be used with User.AutoLogin() later</li>
									<li><strong>Ping Interval Seconds</strong> (default: 0): it's the interval in seconds to send auto-ping to server in order to maintain the online state of local user; to disable auto-ping set this to <em>0</em></li>
									<li><strong>Online Seconds</strong> (default: 120): it's the time in seconds from last action registered to be considered as Online (last action and online state are cached in User class, you will need to reload over time if you need a precise info)</li>
									<li><strong>Playing Seconds</strong> (default: 60): it's the time in seconds from last action to be considered as Playing</li>
									<li><strong>Language</strong> (default: en): the language for server response messages localization</li>
									<li><strong>Achievement UI Object</strong>: the GameObject that handles the user interface of Achievements</li>
									<li><strong>Achievement UI Function</strong>: the name of method to call on <strong>Achievement UI Object</strong> as result to <strong>Social.ShowAchievementsUI</strong></li>
									<li><strong>Leaderboard UI Object</strong>: the GameObject that handles the user interface of Leaderboard</li>
									<li><strong>Leaderboard UI Function</strong>: the name of method to call on <strong>Leaderboard UI Object</strong> as result to <strong>Social.ShowLeaderboardUI</strong></li>
									</ul>
*/

/*! \page page_email Email configuration and Reset Password
    \tableofcontents
    \brief In this section you will learn how to setup your server to send email to clients and reset the user's password.

    \section page_email_server Server configuration
    To enable sending mails to your users you need to set <strong>REGISTER_EMAIL_REQUIRED</strong> to <strong>TRUE</strong> in <em>/lib/config.php</em> and start to store the users email address.

    If you want to send an email as welcome after registration then you also need to set <strong>REGISTER_EMAIL_ACTIVATION</strong> to <strong>TRUE</strong>,
    then set your custom message for subject in <strong>REGISTER_EMAIL_SUBJECT</strong>
    and a file name (it must exist in <em>/email_templates</em>) in <strong>REGISTER_EMAIL_MESSAGE</strong>.

    We suggest to create a copy of <em>/email_templates/email_register.html</em>
    and use it as your own, same for any other sample files that you find in <em>/email_templates</em> which may be overwritten during updates).

    The built-in method <strong>User.ResetPassword</strong> (to reset the password of a user when it has been lost) sends a random generated code by email to the user, so this feature also requires <strong>REGISTER_EMAIL_REQUIRED</strong> to <strong>TRUE</strong>.
    In your interface you need a frame where he can input the code received by email and you call <strong>User.ChangePassword</strong> like here:
\code{.cs}
// Generate a random code and send to the user's email address
User.ResetPassword("username", (bool success, string error) =>
{
    Debug.Log(success + " -- " + error);
});
// User has received the code 'codeReceivedByEmail' by email after ResetPassword
User.ChangePassword(0, "username", "codeReceivedByEmail", "newPasswordChosen", (bool success, string error) =>
{
    Debug.Log("Success: " + success + " --- Error: " + error);
});
\endcode
    You can customize the email sent to the user by setting <strong>RESETPWD_EMAIL_SUBJECT</strong> and <strong>RESETPWD_EMAIL_MESSAGE</strong> (same as above for <em>REGISTER_EMAIL_*</em>).
*/

/*! \page page_security Server-Client Security
	\tableofcontents
	\brief Let's take a look at the implementation of security in Combu

	\section sec_security_keys RSA + AES keys handshaking
	What happens when the Unity client starts:
	<ol>
		<li><strong>CombuManager</strong> calls a web service to initialize the connection to the server (remember we talk about "connection" but HTTP is an asynchronous protocol, so we haven't a real-time synchronized connection like it could be in TCP)</li>
		<li>the server generates private and public RSA keys on the fly for the incoming connection and returns back the public key to the client</li>
		<li>the client then generates AES keys and send them back to the server encrypting them with the RSA public key received</li>
		<li>now both the server and client have the same AES keys that can be used to encrypt/decrypt the content of every web serbice and the client is ready to work</li>
	</ol>
	The reason why we added a double layer of security and don't use only RSA is because RSA encryption/decryption requires more resources (and so time) than AES, besides the fact it effectively adds more security and standard management for such situations.

	\section sec_security_request Data encryption
	Every call to the web services after the initialization is sent to the server encrypted with the AES keys, here is an example of request:
	<pre>http://yourserver/your_combu_path/server.php?token=Q0ItNThhNmQzNjVlMzlkYTguMTIzNTgxNDc%3d&data=2h9Ta4X9Rfdt2HhfqyMdMutsNAWjE%2fsglMh1JBUhS3vnSDO3KqJ7zxZo1icXQ%2fNxHvbbC1lc%2fHsvWsWjxJ4Csg%3d%3d</pre>

	The server response is also encrypted with the same keys if RESPONSE_ENCRYPTED is defined and set to TRUE in <em>config.php</em>:
	<pre>{"t":"2017-04-18 20:32:09","d":"vWH4DEOwj+GI34QOgHzuYWR\/e8rqqlG7hOZxW5nQYzPG6Cv4aQ6BNY4L4T5LruFZALLWgk6lvkMC5gIy2K9dCBlLq1R0QI6mQEFvxNmip28zjOT4eytTAU="}</pre>
*/

/*! \page page_localization Error messages localization
	\tableofcontents
	\brief Localize the error messages sent from server

	\section sec_localization_client Client request
	You can decide in what language the server will output the error messages by setting the property <strong>language</strong> in the <strong>CombuManager</strong> instance.

	\section sec_localization_server Server response
	The default language for messages output is 'en' (english), but if the client sends a specific language request then it is used instead. The file <span style="background:#fafafa;padding:5px;"><strong>'errors_{language}.php'</strong></span> must exist in the folder <em>/error_messages</em> of the server.
*/

/*! \page page_users Authentication and Users
	\tableofcontents
	\brief In this section you will learn how to authenticate the local user, create a new user and load your users.

	\section sec_users_login Authentication
	To authenticate the local user you need to call <strong>CombuManager.instance.platform.Authenticate</strong>:
\code{.cs}
CombuManager.platform.Authenticate( "username", "password", (bool success, string error) => {
	if (success)
		Debug.Log("Login success: ID " + CombuManager.localUser.id);
	else
		Debug.Log("Login failed: " + error);
});
\endcode
	You can store the last successful login credentials when <strong>Remember Credentials</strong> is set to <strong>true</strong> on <strong>CombuManager</strong> inspector (or programmatically), the credentials will be stored in PlayerPref (password encrypted) to be used later with <strong>User.AutoLogin()</strong> method.

	\subsection subsec_users_login_session Auto-login
	If you want to auto-login a previously saved session then you must have stored the credentials by setting <strong>Remember Credentials</strong> to <strong>true</strong> on the instance of <strong>CombuManager</strong> and then call <strong>User.AutoLogin()</strong>:
\code{.cs}
User.AutoLogin ((bool success, string error) => {
	Debug.Log ("AutoLogin: " + success + " -- " + error);
});
\endcode
    You can also check if Autologin is possible (that is username and password were stored before) by using <strong>User.CanAutoLogin</strong>:
\code{.cs}
string storedUsername, storedPassword;
if (User.CanAutoLogin(out storedUsername, out storedPassword)) {
    User.AutoLogin ((bool success, string error) => {
        Debug.Log ("AutoLogin: " + success + " -- " + error);
    });
}
\endcode

	\section sec_users_register Registration
	To create a new user you need to create a new instance of <strong>User</strong> class, set at least <em>username</em> and <em>password</em> and then call <strong>Update</strong> on the instance:
\code{.cs}
User newUser = new User();
newUser.userName = "username";
newUser.password = "password";
newUser.Update( (bool success, string error) => {
	// NB: registration does NOT automatically make the user authenticated
	if (success)
		Debug.Log("Save success: ID " + newUser.id);
	else
		Debug.Log("Save failed: " + error);
});
\endcode

    \subsection subsec_users_register_email Send confirmation by email
	If you set <strong>REGISTER_EMAIL_REQUIRED</strong> to <strong>TRUE</strong> in <em>config.php</em> on your server, then you also need to assign the property <strong>email</strong> of your <em>User</em> object
	and, if also <strong>REGISTER_EMAIL_ACTIVATION</strong> is set to <strong>TRUE</strong>, it will send an email to the new users with a link to activate their account (to prevent spam registrations).

	You can customize the content of the registration email by editing the HTML file <em>/email_templates/email_register.html</em>, for more information please read the constants <em>EMAIL_*</em> at \ref sec_config_server.

	If you want to test the sending of email on your local development server then you need to either install a mail server or set the <em>EMAIL_SMTP*</em> defines in <em>config.php</em>

    About sending out emails, <strong>Combu</strong> uses the library <a href="https://packagist.org/packages/phpmailer/phpmailer" target="_blank"><strong>PHPMailer</strong></a>, so if you're having issues with sending options with SMTP you can create a small PHP test script that uses the PHPMailer class
    (doing a search on google produces tons of examples for the most common free mail servers like gmail) and when you find the correct settings with it then you can bring them in your <em>config.php</em>.

    Here is the correspondence of config.php with PHPMailer properties:
    <ul>
    <li>EMAIL_SMTP: if TRUE then call <em>PHPMailer->isSMTP()</em> to set PHPMailer to use SMTP</li>
    <li>EMAIL_SMTP_HOSTNAME: <em>PHPMailer->Host</em> and <em>PHPMailer->Hostname</em></li>
    <li>EMAIL_SMTP_PORT: <em>PHPMailer->Port</em></li>
    <li>EMAIL_SMTP_SECURE: <em>PHPMailer->SMTPSecure</em></li>
    <li>EMAIL_SMTP_USERNAME: <em>PHPMailer->Username</em> (and set <em>PHPMailer->SMTPAuth</em> to TRUE)</li>
    <li>EMAIL_SMTP_PASSWORD: <em>PHPMailer->Password</em></li>
    </ul>

    Example of <em>/combu/test_mail.php</em>:
\code{.php}
<?php

use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\Exception;

include_once "vendor/autoload.php";

$mail = new PHPMailer(true);
try {

    $mail->isSMTP();
    $mail->Host = 'smtp1.example.com';
    $mail->SMTPAuth = true;
    $mail->Username = 'user@example.com';
    $mail->Password = 'secret';
    $mail->SMTPSecure = 'tls';
    $mail->Port = 587;

    $mail->setFrom('from@example.com', 'Mailer');
    $mail->addAddress('ellen@example.com');

    $mail->isHTML(true);
    $mail->Subject = 'Here is the subject';
    $mail->Body    = 'This is the HTML message body <b>in bold!</b>';
    $mail->AltBody = 'This is the body in plain text for non-HTML mail clients';

    $mail->send();
    echo 'Message has been sent';
} catch (Exception $e) {
    echo 'Message could not be sent. Mailer Error: ', $mail->ErrorInfo;
}

\endcode

	\subsection subsec_users_register_guest Create a guest account
	If you want to create a guest account then you need to call <strong>CreateGuest</strong> on a <strong>User</strong> object, you could activate <strong>rememberCredentials</strong> to use <strong>AutoLogin</strong> later:
\code{.cs}
// First try to auto-login
CombuManager.instance.rememberCredentials = true;
string storedUsername, storedPassword;
if (User.CanAutoLogin(out storedUsername, out storedPassword)) {
	Debug.Log ("Try to auto-login with username '" + storedUsername + "' and password '" + storedPassword + "'");
	User.AutoLogin ((bool loginSuccess, string loginError) => {
		Debug.Log ("AutoLogin: " + loginSuccess + " -- " + loginError);
		if (loginSuccess)
			Debug.Log("Logged in");
	});
	return;
}
// If autologin fails then create a guest account
var user = new User ();
user.CreateGuest ((bool success, string error) => {
	if (success) {
		Debug.Log ("CreateGuest - Success=" + success + " > Error=" + error);
	}
});
\endcode

	\section sec_users_customdata Custom Data
	Each user has a <em>Hashtable</em> property <strong>customData</strong> that you can use to store your own strings, numbers, JSON etc that should be visible from all Apps (for example a Premium/VIP status of the user in your games-hub).

    Recently it was introduced another similar property <strong>appCustomData</strong> that you can use to set the properties, attributes, statistics etc which are relevant to the App that is currently running in the game.

	By using these key/value dictionaries you can create the profile that better fits your need and extend it by code without any limitation at no cost, for example:
\code{.cs}
// Global custom data (all Apps)
CombuManager.localUser.customData["VIP"] = true;
// Local custom data (only current App)
CombuManager.localUser.appCustomData["Nickname"] = "MyNickname";
CombuManager.localUser.appCustomData["Gold"] = 100;
CombuManager.localUser.appCustomData["Experience"] = 1000;
// Send the changes to the server
CombuManager.localUser.Update((bool success, string error) => {
    if (success)
        Debug.Log("Saved");
    else
        Debug.Log("Failed: " + error);
});
\endcode

	\section sec_users_loadusers Load users
	To load the users data you can call <strong>CombuManager.instance.platform.LoadUsers()</strong>,
	or one of the <strong>User.Load()</strong> overloads
	(with <em>User.Load</em> form you will not need to cast back from <strong>IUserProfile</strong> to <strong>User</strong>):
\code{.cs}
// Load a user by Id
User.Load ( 123, ( User user ) => {
	Debug.Log("Success: " + (user == null ? "false" : "true"));
});
// Load a user by userName
User.Load ( "user1", ( User user ) => {
	Debug.Log("Success: " + (user == null ? "false" : "true"));
});
// Load users by Id
User.Load ( new long[] { 123, 456 }, ( User[] users ) => {
	Debug.Log("Loaded: " + users.Length);
});
// Search users by Username, Email, CustomData
// Filter players with custom data "Level" between 5 and 10
SearchCustomData[] searchData = new SearchCustomData[] {
	new SearchCustomData("Level", eSearchOperator.GreaterOrEquals, "5"),
	new SearchCustomData("Level", eSearchOperator.LowerOrEquals, "10")
};
User.Load("part-of-username", "email@server.com", searchData, 1, 1, (User[] users, int resultsCount, int pagesCount) => {
	Debug.Log("Loaded: " + users.Length);
});
\endcode
	You can also load a list of random users (excluding localUser):
\code{.cs}
// Filter players with custom data "Level" between 5 and 10
SearchCustomData[] searchData = new SearchCustomData[] {
	new SearchCustomData("Level", eSearchOperator.GreaterOrEquals, "5"),
	new SearchCustomData("Level", eSearchOperator.LowerOrEquals, "10")
};
User.Random(searchData, 3, (User[] users) => {
	foreach (User user in users)
	{
		if (user.lastSeen == null)
			Debug.Log(user.userName + " Never seen");
		else
		{
			System.DateTime seen = (System.DateTime)user.lastSeen;
			Debug.Log(user.userName + " Last seen: " + seen.ToLongDateString() + " at " + seen.ToLongTimeString() + " - Online state: " + user.state);
		}
	}
});
\endcode

	\section sec_users_onlinestate Online state
	The Profile class implements the <strong>IUserProfile</strong> interface, so it also provides the <em>state</em> property
	to get the online state of your users. Of course remember that we are in an asynchronous environment, so your players are not really connected in real-time
	and if you need to rely on the online state then you will need to implement your own polling system to refresh your lists from time to time.

	To mantain the online state CombuManager uses the settings <em>pingIntervalSeconds</em>, <em>onlineSeconds</em> and <em>playingSeconds</em>.
	Besides the ping function that is called every <em>pingIntervalSeconds</em> seconds (set 0 to disable, anyway we'd recommend to have the interval not too small,
	a value of 30 should be fine else you may suffer of high traffic), every action served by the webservices updates the "last action" date/time of a user.

	\section sec_users_class Create your User class
	Since you're able to extend the basic <strong>User</strong> class with the <em>customData</em> (for global data available to all apps) or <em>appCustomData</em> (for the current app data) properties,
	the best way to work with the system is to create your own class for users by inheriting from <strong>User</strong>.

	This way you can create your own account properties, that for sure are much more readable than <em>customData["myProperty"]</em>
	(especially if you need non-string values, it would be lot of explicit casts or Parse!).

	Remember to:
	<ul>
		<li>set their values in <em>customData</em>, so they will be passed to <strong>Update</strong> and saved to server</li>
		<li>override <strong>FromHashtable</strong> to fill the internal variables from the <em>customData</em> received from server</li>
	</ul>
\code{.cs}
public class CombuDemoUser : Combu.User
{
	string _myProperty1 = "";
	int _myProperty2 = 0;

	public string myProperty1
	{
		get { return _myProperty1; }
		set { _myProperty1 = value; customData["myProperty1"] = _myProperty1; }
	}
	
	public int myProperty2
	{
		get { return _myProperty2; }
		set { _myProperty2 = value; customData["myProperty2"] = _myProperty2; }
	}

	public CombuDemoUser()
	{
		myProperty1 = "";
		myProperty2 = 0;
	}

	public override void FromHashtable (Hashtable hash)
	{
		// Set User class properties
		base.FromHashtable (hash);

		// Set our own custom properties that we store in customData
		if (customData.ContainsKey("myProperty1"))
			_myProperty1 = customData["myProperty1"].ToString();
		if (customData.ContainsKey("myProperty2"))
			_myProperty2 = int.Parse(customData["myProperty2"].ToString());
	}
}
\endcode
	To use the new class in your code, you will need to pass the referenced type to the user-wise methods:
\code{.cs}
// Create new user
CombuDemoUser newUser = new CombuDemoUser();
newUser.userName = "username";
newUser.password = "password";
newUser.myProperty1 = "Value";
newUser.myProperty2 = 100;
newUser.Update( (bool success, string error) => {
	// NB: registration does not make the user logged
	if (success)
		Debug.Log("Save success: ID " + newUser.id);
	else
		Debug.Log("Save failed: " + error);
});
// Authenticate user
CombuManager.platform.Authenticate <CombuDemoUser> ( "username", "password", (bool success, string error) => {
    if (success) {
        // Now you can safely cast localUser
        CombuDemoUser user = CombuManager.localUser as CombuDemoUser;
        Debug.Log("Success: " + user.myProperty2);
    } else {
        Debug.Log("Failed: " + error);
    }
});
\endcode
*/

/*! \page page_settings Server Settings
	\tableofcontents
	\brief In this section you will learn how to create server settings and access them from client.

	\section sec_settings_server Server
	You can create server settings in the administration console, in the section <strong>Settings</strong>.

	\section sec_settings_client Client
	The client automatically loads the settings in <strong>CombuManager.instance.serverInfo.settings</strong>:
\code{.cs}
while (!CombuManager.isInitialized)
	yield return null;
string mySetting = CombuManager.instance.serverInfo.settings["mySetting"].ToString();
\endcode
*/

/*! \page page_extplatforms Linking external platforms
	\tableofcontents
	\brief In this section you will learn how to authenticate and link the local user to an external platform like GameCenter, Facebook etc.

	\section sec_extplatforms_login Authentication
	To authenticate the local user with a platform Id you need to call <strong>CombuManager.localUser.AuthenticatePlatform</strong>.
	If the platform key+id exists then it will return the registered account, else it will create a new account with username <strong>PlatformName_PlatformId</strong> (including the underscore symbol).
\code{.cs}
// After you have logged in with Facebook SDK (http://u3d.as/5j1)
CombuManager.localUser.AuthenticatePlatform("Facebook", FB.UserId, (bool success, string error) => {
	if (success)
		Debug.Log("Login success: ID " + CombuManager.localUser.id);
	else
		Debug.Log("Login failed: " + error);
});
\endcode

	\section sec_extplatforms_linkplatform Link a platform to the local user
	If the local user is already logged, you can link a platform Id to the account with <strong>CombuManager.localUser.LinkPlatform</strong>:
\code{.cs}
CombuManager.localUser.LinkPlatform("YourPlatformName", "YourPlatformId", (bool success, string error) => {
	if (success)
		Debug.Log("Link success");
	else
		Debug.Log("Link failed: " + error);
});
\endcode

	\section sec_extplatforms_linkaccount Transfer the external platforms
	Sometimes may happen that you need to move the external platforms of the local user to another account,
	in this case you can use <strong>CombuManager.localUser.LinkAccount</strong> (the platforms key+id of the local user account will be transferred to the new account,
	the account and all its data/scores/etc deleted, and the new account will be assigned to the local user):
\code{.cs}
CombuManager.localUser.LinkAccount("other_username", "other_password", (bool success, string error) => {
	if (success)
		Debug.Log("Transfer success");
	else
		Debug.Log("Transfer failed: " + error);
});
\endcode

    \section sec_extplatforms_search Load users from platforms
    If your users have logged with their platform profile then you can retrieve their Combu profiles later (for example after a call to Facebook API that returns the list of friends to show who is playing your game by matching the existance of a profile in Combu):
\code{.cs}
// Assign platform and related id to all users that you want to search, for example coming from Facebook friends API
List<string> platforms = new List<string>() { "Facebook", "Facebook" }; // you have passed "Facebook" as platform key to AuthenticatePlatform
List<string> ids = new List<string>() { "000000", "111111" };
User.LoadPlatform(platforms, ids, (User[] users) => {
    Debug.Log("Users found: " + users.Length);
});
\endcode
*/

/*! \page page_contacts Managing Contacts
	\tableofcontents
	\brief In this section you will learn how to retrieve the contacts lists of the local user and how to add/remove users to the friends/ignore lists.

	\section sec_contacts_list Loading Contacts
	To retrieve the list of contacts from the local user you need to call the <em>LoadFriends</em> method on <strong>CombuManager.localUser</strong>
	and then access to the results list accordingly to the eContactType value passed to the function (<strong>CombuManager.localUser.friends</strong> for <strong>eContactType.Friend</strong>,
	<strong>CombuManager.localUser.requests</strong> for <strong>eContactType.Request</strong>, <strong>CombuManager.localUser.ignored</strong> for <strong>eContactType.Ignore</strong>,
	<strong>CombuManager.localUser.pendingRequests</strong> for <strong>eContactType.PendingRequest</strong>):
\code{.cs}
CombuManager.localUser.LoadFriends( eContactType.Friend, (bool success) => {
	if (success)
		Debug.Log("Success: " + CombuManager.localUser.friends.Length);
	else
		Debug.Log("Failed");
});
\endcode

	\section sec_contacts_add Adding Contacts
	To add another user to a contact list of the local user you need to call <em>AddContact</em> and pass either a User/Profile object or a username and <em>eContactType.Friend</em> as contact type:
\code{.cs}
// Add by User/Profile object
CombuManager.localUser.AddContact(otherUser, eContactType.Friend, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Add by username
CombuManager.localUser.AddContact("username", eContactType.Friend, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_contacts_remove Removing Contacts
	To remove a user from the contact lists of the local user you need to call <em>RemoveContact</em>:
\code{.cs}
// Remove by User/Profile object
CombuManager.localUser.RemoveContact(otherUser, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Remove by username
CombuManager.localUser.RemoveContact("username", (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/

/*! \page page_files Managing Files
	\tableofcontents
	\brief In this section you will learn how to retrieve the files lists of the local user or anyway accessible by him and how to add/remove files.

	\section sec_files_list Loading Files
	To retrieve a list of files you need to call <strong>UserFile.Load</strong>:
\code{.cs}
bool includeShared = true;
int pageNumber = 1;
int countPerPage = 10;
UserFile.Load(CombuManager.localUser.id, includeShared, pageNumber, countPerPage, (UserFile[] files, int resultsCount, int pagesCount, string error) => {
	if (string.IsNullOrEmpty(error))
		Debug.Log("Files loaded: " + files.Length);
	else
		Debug.Log(error);
});
\endcode

	\section sec_files_add Adding Files
	To add a file associated to the local user you need to create a new instance of <strong>UserFile</strong>, set <em>sharing</em> property and call <strong>Update</strong>:
\code{.cs}
byte[] screenshot = CombuManager.instance.CaptureScreenShot();
UserFile newFile = new UserFile();
newFile.sharing = UserFile.eShareType.Everybody;
newFile.customData ["Prop"] = "Value";
newFile.Update(screenshot, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_files_view Viewing Files
	To increase the <strong>View</strong> count of a file you need to call <strong>UserFile.View</strong>, or call the method <strong>View</strong> on a <strong>UserFile</strong> instance:
\code{.cs}
// View by File ID
UserFile.View(123, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// View by UserFile object
myFile.View( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_files_like Liking Files
	To increase the <strong>Like</strong> count of a file you need to call <strong>UserFile.Like</strong>, or call the method <strong>Like</strong> on a <strong>UserFile</strong> instance:
\code{.cs}
// Like by File ID
UserFile.Like(123, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Like by UserFile object
myFile.Like( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_files_remove Removing Files
	To remove a file owned by the local user you need to call <strong>UserFile.Delete</strong>, or call the method <strong>Delete</strong> on a <strong>UserFile</strong> instance:
\code{.cs}
// Remove by File ID
UserFile.Delete(123, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Remove from a UserFile object
myFile.Delete( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/

/*! \page page_inventory Managing Inventory
	\tableofcontents
	\brief In this section you will learn how to manage the inventory of the local user.

	\section sec_inventory_list Loading Inventory
	To retrieve the list of items in the inventory of a user you need to call the <strong>Inventory.Load</strong>:
\code{.cs}
// Load the inventory of user ID '123'
Inventory.Load( "123", (Inventory[] items, string error) => {
	if (success)
		Debug.Log("Success: " + items.Length);
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_inventory_add Adding and Editing Items
	To add a new item in the inventory of the local user you need to create a new <strong>Inventory</strong> instance, set <em>name</em>, <em>quantity</em> and <em>customData</em> and then call <strong>Update</strong>:
\code{.cs}
// Add a new item
Inventory newItem = new Inventory();
newItem.name = "My item";
newItem.quantity = 1;
newItem.customData["Durability"] = 100;
newItem.Update( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Edit an item previously loaded
myItem.quantity--;
myItem.Update( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_inventory_remove Removing Items
	To remove an item from the inventory of the local user you need to call <strong>Inventory.Delete</strong>:
\code{.cs}
// Remove by inventory ID '123'
Inventory.Delete(123, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Remove by Inventory object
myItem.Delete( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/

/*! \page page_mail Managing Messages
	\tableofcontents
	\brief In this section you will learn how to manage the in-game inbox of the local user.

	\section sec_mail_list Loading Messages
	To retrieve the list of messages of a user you need to call the <strong>Message.Load</strong>:
\code{.cs}
// Load the received messages from the page 1 with 3 results per page
Mail.Load(eMailList.Received, 1, 3, (Mail[] messages, int count, int pagesCount, string error) => {
	if (string.IsNullOrEmpty(error))
		Debug.Log("Success: " + messages.Length);
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_mail_add Sending Messages
	To send a new message to a user you need to call one of the overloads of <strong>Mail.Send</strong>:
\code{.cs}
// Send a private message to a user by Id
Mail.Send(123, "Subject", "Message body", false, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Send a private message to a user by Username
Mail.Send("user1", "Subject", "Message body", false, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Send a private message to multiple users by Id
Mail.Send( new long[] { 123, 456 }, "Subject", "Message body", false, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Send a private message to multiple users by Username
Mail.Send( new string[] { "user1", "user2" }, "Subject", "Message body", false, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\subsection subsec_mail_sendgroup Sending to UserGroup
	If you want to send a message to a UserGroup then you need to call <strong>Mail.SendMailToGroup</strong>:
\code{.cs}
// Send a message to the UserGroup ID '123'
Mail.SendMailToGroup(123, "Subject", "Message body", false, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_mail_read Marking Messages as Read
	To mark a message as <strong>Read</strong> you need to call <strong>Mail.Read</strong>, or call the method <strong>Read</strong> on a <strong>Mail</strong> instance:
\code{.cs}
// Mark as Read by Mail ID
Mail.Read( new long[] { 123, 456 }, new long[0], (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Mark as Read by Group ID
Mail.Read( new long[0], new long[] { 123, 456 }, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Mark as Read by Mail object
myMail.Read( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

\section sec_mail_unread Marking Messages as Unread
To mark a message as <strong>Unread</strong> you need to call <strong>Mail.Unread</strong>, or call the method <strong>Unread</strong> on a <strong>Mail</strong> instance:
\code{.cs}
// Mark as Unread by Mail ID
Mail.Unread( 123, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Mark as Unread by Mail object
myMail.Unread( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_mail_remove Removing Items
	To delete a message you need to call <strong>Message.Delete</strong>, or call the method <strong>Delete</strong> on a <strong>Mail</strong> instance:
\code{.cs}
// Remove by Message ID '123'
Mail.Delete(123, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Remove by Mail object
myMail.Delete( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_mail_conv Loading Conversations
	To load the list of discussions of the local user with others you can call <strong>Mail.LoadConversations</strong>,
	it will fill an <strong>ArrayList</strong> with objects of type <strong>User</strong> or <strong>UserGroup</strong> (based on the <em>idGroup</em> associated to the <strong>Mail</strong> object):
\code{.cs}
Mail.LoadConversations( (ArrayList conversations, int count, string error) => {
	if (string.IsNullOrEmpty(error))
		Debug.Log("Success: " + conversations.Count);
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_mail_count Counting Messages
	To count the messages in the inbox of the local user you can call <strong>Mail.Count</strong>:
\code{.cs}
// Count the messages sent from users ID
Mail.Count( new long[] { 123, 456 }, new long[0], (MailCount[] counts, string error) => {
	if (string.IsNullOrEmpty(error))
		Debug.Log("Success: " + counts.Length);
	else
		Debug.Log("Failed: " + error);
});
// Count the messages sent from groups ID
Mail.Count( new long[0], new long[] { 123, 456 }, (MailCount[] counts, string error) => {
	if (string.IsNullOrEmpty(error))
		Debug.Log("Success: " + counts.Length);
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/

/*! \page page_groups Managing User Groups
	\tableofcontents
	\brief In this section you will learn how to manage the user groups.

	\section sec_groups_create Create a new group
	You can create a new group with <strong>UserGroup.Save</strong> (call this also to edit a group that was loaded):
\code{.cs}
UserGroup group = new UserGroup();
group.name = "My Group";
// Add some users
group.users = new User[] { {userName="OtherUser1"}, {userName="OtherUser2"} };
group.Save((bool success, string error) => {
	Debug.Log("Group saved: " + success);
});
\endcode

	\section sec_groups_load Load groups
	To load groups you have 3 choices:
	<ul>
		<li>load groups owned by local user
\code{.cs}
UserGroup.Load(CombuManager.localUser.idLong, (UserGroup[] groups, string error) => {
	Debug.Log(groups.Length);
});
\endcode
</li>
		<li>load groups in which local user is a member (owners are also members)
\code{.cs}
UserGroup.LoadMembership(CombuManager.localUser.idLong, (UserGroup[] groups, string error) => {
	Debug.Log(groups.Length);
});
\endcode
</li>

	\section sec_groups_join Join a group
	You can join a group with <strong>UserGroup.Join</strong>:
\code{.cs}
// Join local user
group.Join((bool success, string error) => {
	Debug.Log("Group joined: " + success);
});
// Join a list of users
group.Join(new string[] {"user1"}, (bool success, string error) => {
	Debug.Log("Group joined: " + success);
});
\endcode

	\section sec_groups_leave Leave a group
	You can leave a group with <strong>UserGroup.Leave</strong>:
\code{.cs}
// Leave local user
group.Leave((bool success, string error) => {
	Debug.Log("Group joined: " + success);
});
// Leave a list of users
group.Leave(new string[] {"user1"}, (bool success, string error) => {
	Debug.Log("Group joined: " + success);
});
\endcode
*/

/*! \page page_news Managing News
	\tableofcontents
	\brief In this section you will learn how to retrieve the in-game news.

	\section sec_news_list Loading News
	To retrieve the list of latest news you need to call <strong>News.Load</strong>:
\code{.cs}
// Load the page 1 with 10 results per page
News.Load(1, 10, (News[] news, int resultsCount, int pagesCount, string error) => {
	if (string.IsNullOrEmpty(error))
		Debug.Log("Success: " + news.Length);
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/

/*! \page page_leaderboard Managing Leaderboards
	\tableofcontents
	\brief In this section you will learn how to access the leaderboards data and report a score.

	\section sec_leaderboard_list Loading Scores
	To retrieve the scores list of a leaderboard you can call <strong>CombuManager.platform.LoadScores</strong> (by default loads the first page with 10 results), or you can instantiate
	a new <strong>Leaderboard</strong> object (created with <strong>CombuManager.platform.CreateLeaderboard</strong>), set <em>timeScope</em>, <em>customTimescope</em> and <em>range</em> and then call <strong>LoadScores</strong>:
\code{.cs}
// Load the scores of the leaderboard ID '123' from the page 2 with 10 results per page
CombuManager.platform.LoadScores(123, 2, 10, (IScore[] scores) => {
	Debug.Log("Scores loaded: " + scores.Length);
});
// Load the scores of the leaderboard ID '123' from the page 1 with 10 results per page
ILeaderboard board = CombuManager.platform.CreateLeaderboard ();
board.id = "123";
board.timeScope = UnityEngine.SocialPlatforms.TimeScope.AllTime;
board.range = new UnityEngine.SocialPlatforms.Range(1, 10);
board.LoadScores((bool loaded) => {
	// Now you can access board.scores and board.localUserScore
});
\endcode

	\section sec_leaderboard_add Reporting Scores
	To report a new score of the local user you need to call <strong>CombuManager.platform.ReportScore</strong>, or you can call the method <strong>ReportScore</strong> on a <strong>Score</strong> instance:
\code{.cs}
// Report a score of 1000 to the leaderboard ID '123'
CombuManager.platform.ReportScore(1000, "123", (bool success) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed");
});
// Report a score of 1000 on a Score object,
// (previously loaded with LoadScores or a new with leaderboardID set)
myScore.value = 1000;
myScore.ReportScore( (bool success) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed");
});
\endcode

	\section sec_leaderboard_load Loading Leaderboards data
	To load the leaderboards data like the title, description etc, you need to call <strong>Leaderboard.Load</strong>:
\code{.cs}
// Load the leaderboard ID '123'
Leaderboard.Load("123", (Leaderboard leaderboard, string error) => {
	if (leaderboard != null)
		Debug.Log("Success: " + leaderboard.title);
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_leaderboard_userscore Loading the score of a user
	To retrieve the best score of a user you need to call <strong>Leaderboard.LoadScoresByUser</strong>:
\code{.cs}
// Load the best score of leaderboard ID '123' for the User object with 10 results per page (to be attendible 'page', the limit must be the same as used with LoadScores)
Leaderboard.LoadScoresByUser("123", user, eLeaderboardInterval.Week, 10, (Score score, int page, string error) => {
	if (score != null)
		Debug.Log("Success: " + score.value + " at page " + page);
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/

/*! \page page_achievement Managing Achievements
	\tableofcontents
	\brief In this section you will learn how to load the achievements and report a progress.

	\section sec_achievement_list Loading Achievements descriptions
	To retrieve the list of achievements you can call <strong>CombuManager.platform.LoadAchievementDescriptions</strong>
	or <strong>CombuManager.platform.LoadAchievements</strong> (the latter form is preferred because you will not need to cast back from <em>IAchievement</em> to <em>Achievement</em>):
\code{.cs}
// Load the achievement descriptions
CombuManager.platform.LoadAchievements( (Achievement[] achievements) => {
	Debug.Log("Achievements loaded: " + achievements.Length);
});
\endcode

	\section sec_achievement_add Reporting Progress
	To report a new progress of the local user you need to call <strong>CombuManager.platform.ReportProgress</strong>, or you can call the method <strong>ReportProgress</strong> on a <strong>Score</strong> instance:
\code{.cs}
// Report a score of 1000 to the achievement ID '123'
CombuManager.platform.ReportProgress(1000, "123", (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Report a progress of 30% on an Achievement object,
// (previously loaded with LoadAchievements or created with <strong>CombuManager.platform.CreateAchievement</strong> and with <em>id</em> set)
myAchievement.percentCompleted = 0.3;
myAchievement.ReportProgress( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/

/*! \page page_tournaments Managing Tournaments
	\tableofcontents
	\brief In this section you will learn how to retrieve the tournaments list of the local user and how to add/remove tournaments.

	\section sec_tournaments_list Loading Tournaments
	To retrieve the list of tournaments accessible by the local user you need to call <strong>Tournament.Load</strong>:
\code{.cs}
// Load the active tournaments, no filter by customData
Tournament.Load(false, null, (Tournament[] tournaments) => {
	Debug.Log("Tournaments loaded: " + tournaments.Length);
});
\endcode

	\subsection subsec_tournaments_load Loading by Tournament ID
	You can also load a single Tournament by its ID:
\code{.cs}
// Load a Tournament by ID
Tournament.Load(123, (Tournament tournament) => {
	if (tournament != null)
		Debug.Log("Success: " + tournament.title);
	else
		Debug.Log("Failed");
});
\endcode

	\subsection subsec_tournaments_matches Matches of the Tournament
	Once that the Tournament has been loaded, the property <em>matches</em> gives you access to its matches and their extra data.

	\section sec_tournaments_add Quick Tournament
	To create a quick tournament with other users user you need to call <strong>Tournament.QuickTournament</strong>:
\code{.cs}
// Load 2 random users
User.Random( null, 2, (User[] users) => {
	Tournament t = Tournament.QuickTournament(users);
	if (t.matches.Count == 0)
	{
		Debug.Log("Something going wrong, no matches created");
	}
	else
	{
		t.title = "My Tournament 1";
		t.customData["Key1"] = "Value";
		
		t.Save((bool success, string error) => {
			
			Debug.Log("Success: " + success + " - Matches: " + t.matches.Count);
			
		});
	}
});
\endcode

	\subsection subsec_tournaments_customize Create your own Tournament
	You can take a look at the code in <strong>Tournament.QuickTournament</strong> to see how to create a <strong>Tournament</strong>
	and use the code as base to create your own type of tournaments.

	\section sec_tournaments_remove Removing Tournaments
	To delete a tournament you need to call <strong>Tournament.Delete</strong>, or call the method <strong>Delete</strong> on a <strong>Tournament</strong> instance:
\code{.cs}
// Remove by Tournament ID
Tournament.Delete(123, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Remove by Tournament object
myTournament.Delete( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/

/*! \page page_matches Managing Matches
	\tableofcontents
	\brief In this section you will learn how to retrieve the matches list of the local user and how to add/remove matches.

	\section sec_matches_list Loading Matches
	To retrieve the list of matches of the local user you need to call <strong>Match.Load</strong>:
\code{.cs}
// Load the active matches, no Tournament ID, no filter by title
Match.Load(0, true, string.Empty, (Matches[] matches) => {
	Debug.Log("Matches loaded: " + matches.Length);
});
\endcode

	\subsection subsec_matches_load Loading by Match ID
	You can also load a single Matches by its ID:
\code{.cs}
// Load a Matches by ID
Match.Load(123, (Match match) => {
	if (match != null)
		Debug.Log("Success: " + match.title);
	else
		Debug.Log("Failed");
});
\endcode

	\subsection subsec_matches_rounds Rounds of the Match
	Once that the Match has been loaded, the property <em>rounds</em> gives you access to its rounds and scores:
	the collection <em>users</em> contains the <strong>MatchAccount</strong> objects (relationship between Match and Account),
	the collection <em>scores</em> contains the <strong>MatchRound</strong> objects (relationship between the Match-Account association and a round).

	\section sec_matches_add Quick Match
	To create a quick match with another user you need to call <strong>Match.QuickMatch</strong>:
\code{.cs}
// Load 2 random users (not only friends), no filter by customData, 1 round
Match.QuickMatch(false, null, 1, (Match match) => {
	if (match != null)
		Debug.Log("Success: " + match.title);
	else
		Debug.Log("Failed");
});
\endcode

	\subsection subsec_matches_customize Create your own Match
	You can take a look at the code in <strong>Match.QuickMatch</strong> to see how to create a <strong>Match</strong>
	and use the code as base to create your own matches.

	\section sec_matches_score Sending Score
	To send a score for the next round you need to call the method <strong>Score</strong> on a <strong>Match</strong> instance:
\code{.cs}
// Send a score of 1000
myMatch.Score(1000, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_matches_remove Removing Matches
	To delete a match you need to call <strong>Match.Delete</strong>, or call the method <strong>Delete</strong> on a <strong>Match</strong> instance:
\code{.cs}
// Remove by Match ID
Match.Delete(123, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Remove by Match object
myMatch.Delete( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/

/*! \page page_customize_admin Customize Web Administration
	\tableofcontents
	\brief Learn how to customize the web administration through CSS and JS

	\section sec_customize_admin_js Javascript
	You can add your own JS code by creating a file called <strong>custom.js</strong> in the folder <strong>/admin/js</strong>,
	if this file exists then it will be automatically added to the HTML output of the web administration.

	\section sec_customize_admin_css CSS
	You can override any CSS class by creating a file called <strong>custom.css</strong> in the folder <strong>/admin/css</strong>,
	if this file exists then it will be automatically added to the HTML output of the web administration.

	For example to change the Combu logo displayed at top left of every page with your own, you can upload your own logo
	into the folder <strong>/admin/images</strong> and create the file <strong>/admin/css/custom.css</strong> with the following content:
\code{.unparsed}
#logo-title {background-image: url('../images/your_logo.png');}
\endcode
 */

/*! \page page_faq Frequently Asked Questions
   \tableofcontents
   \brief Read the FAQ if you're having issues before sending a post on forum.

   \section page_faq_1 How do I install Combu on my local/live server?
   Read the section \ref page_server and \ref page_config.

   \section page_faq_2 How can I add my own properties to accounts?
   In <strong>Combu</strong> you don't need to manually modify the core user class or database table to add your own properties to accounts like <em>FirstName</em>, <em>LastName</em>, <em>Coins</em> and so on.

   All you need is to use user's <strong>customData</strong>: it's a Hashtable that can store every data you need. Let's see how you can extend the core user class and add a couple of custom properties as example.

   First you need to create a class inheriting from <em>User</em>:
\code{.cs}
using System.Collections;
using Combu;

public class CombuDemoUser : User
{
   string _myProperty1 = "";
   int _myProperty2 = 0;
   int _coins = 0;

   public string myProperty1
   {
       get { return _myProperty1; }
       set { _myProperty1 = value; customData["myProperty1"] = _myProperty1; }
   }

   public int myProperty2
   {
       get { return _myProperty2; }
       set { _myProperty2 = value; customData["myProperty2"] = _myProperty2; }
   }

   public int coins
   {
       get { return _coins; }
       set { _coins = value; customData["Coins"] = _coins; }
   }

   public CombuDemoUser()
   {
       myProperty1 = "";
       myProperty2 = 0;
       coins = 0;
   }

   public override void FromHashtable (Hashtable hash)
   {
       // Set User class properties
       base.FromHashtable (hash);

       // Set our own custom properties that we store in customData
       if (customData.ContainsKey("myProperty1"))
           _myProperty1 = customData["myProperty1"].ToString();
       if (customData.ContainsKey("myProperty2"))
           _myProperty2 = int.Parse(customData["myProperty2"].ToString());
       if (customData.ContainsKey("Coins"))
           _coins = int.Parse(customData["Coins"].ToString());
   }
}
\endcode
   Now we can use this new class for Authenticate and AutLogin to safely cast <strong>CombuManager.instance.loggedUser</strong> after a successful login (similar syntax can be used for other methods like <strong>User.AutoLogin</strong> or <strong>CombuManager.LoadContacts</strong>):
\code{.cs}
CombuManager.platform.Authenticate<CombuDemoUser> (username, password, (bool loginSuccess, string loginError) => {
   CombuDemoUser myUser = (CombuDemoUser)CombuManager.localUser;
   Debug.Log("Login: " + loginSuccess + " -- " + loginError);
   if (loginSuccess)
       Debug.Log("Property1: " + myUser.myProperty1);
});
\endcode

   \section page_faq_3 I purchased it on Asset Store, can I download from your website?
   Yes, you can redeem your Unity invoice (both server and addons packages) from <a href="https://www.skaredcreations.com/wp/redeem-asset-store-invoice/" target="_blank">here</a>
   and you will get free access to all downloads that you purchased on Asset Store.

   \section page_faq_4 When I try to navigate to the web admin, I see a blank page
   If you see a blank page then there's probably an internal server error and your web server is configured to hide errors from web pages,
   so your first step is to enable "display_errors" variable in <em>php.ini</em> or <em>.htaccess</em> (search on internet or ask to your hosting provider how to do).

   Remember the requirements that your web server should meet:
   <ul>
       <li>PHP version must be 5.5 or greater</li>
       <li>the zip extension module must be enabled on PHP (if it isn't enabled then the auto-updater will not be able to uncompress and install the updates, but you can still use it to automatically execute the SQL queries if required)</li>
       <li>to successfully use the auto-updater feature, the server user (Apache user on OSX/Linux or IUSR/NETWORK_SERVICE on Windows) must have write permissions on Combu root folder</li>
   </ul>

   Also make sure you have set the correct connection settings in your <em>/lib/config.php</em>.
*/