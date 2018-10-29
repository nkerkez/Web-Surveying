using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using WebSurveying2017.Providers;
using WebSurveying2017.Models;
using WebSurveying2017.Data;
using Microsoft.AspNet.Identity.Owin;
using WebSurveying2017.FacebookService;
using Microsoft.Owin.Security.Facebook;
using System.Threading.Tasks;
using System.Security.Claims;
using Google.Apis.Oauth2.v1;

namespace WebSurveying2017
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(WebSurveyingContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true
                
            };
            

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            var googleOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "34988273456-ispl3p5obt41ro6jc4dlco718ua2m24o.apps.googleusercontent.com",
                ClientSecret = "oTxPJZH8CjhX6NF_5WXPsO2S"
            };



            /*,
            Events = new OAuthEvents
            {
                OnCreatingTicket = context =>
                {
                    // Extract the "language" property from the JSON payload returned by
                    // the user profile endpoint and add a new "urn:language" claim.
                    var language = context.User.Value<string>("language");
                    context.Identity.AddClaim(new Claim("urn:language", language));

                    return Task.FromResult(0);
                }

        }*/
            

            googleOptions.Provider = new GoogleOAuth2AuthenticationProvider()
            {
                OnAuthenticated = (context) =>
                {
                    foreach (var claim in context.User)
                    {
                        var claimType = string.Format("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/{0}", claim.Key);
                        string claimValue = claim.Value.ToString();
                        context.Identity.AddClaim(new Claim(claimType, claimValue, "http://www.w3.org/2001/XMLSchema#string"));
                    }

                    return Task.FromResult(0);
                }
            };
                

            


            var facebookOptions = new FacebookAuthenticationOptions()
            {
                AppId = "320144205128698",
                AppSecret = "9ca374c688b412a40c46c043d9632c6d",
                BackchannelHttpHandler = new FacebookBackChannelHandler(),
                
                UserInformationEndpoint = "https://graph.facebook.com/v2.12/me?fields=id,email,first_name,last_name,gender",
                Provider = new FacebookAuthenticationProvider
                {
                    OnAuthenticated = context =>
                    {
                        context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
                        return Task.FromResult(true);
                    }
                }
            };



      
            facebookOptions.Scope.Add("email");
            app.UseFacebookAuthentication(facebookOptions);
            /*
            googleOptions.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
            googleOptions.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
            googleOptions.Scope.Add("https://www.googleapis.com/auth/plus.login");
            */
            app.UseGoogleAuthentication(googleOptions);

        }
    }
}
