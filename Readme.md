#doc-identity

This project is a subset of [docStack](https://github.com/schwamster/docStack)

It is realized with [IdentityServer4](https://github.com/IdentityServer/IdentityServer4/blob/release/docs/index.rst)

## State

[![CircleCI](https://circleci.com/gh/schwamster/doc-identity.svg?style=shield&circle-token)](https://circleci.com/gh/schwamster/doc-identity)
[![Docker Automated buil](https://img.shields.io/docker/automated/jrottenberg/ffmpeg.svg)](https://hub.docker.com/r/schwamster/doc-identity/)

## Getting started
Pull the image from docker and run it. Set the environment variable *"Identity:AdminPassword"* to a secure password for the container. Alternatevly
set the password in the respective setting in appsettings.json if you run the app "directly". 

Right now this is a none generic identity server for the [docStack](https://github.com/schwamster/docStack) project.
In order for this to work you will also have to add further configuration as environment variables:

* Identity:DocStackAppHost => host and port of the frontend app "doc-stack-app"

## Adding more Clients

Right now only the doc-stack-app is configured as a client and only the doc-stack-app-api is registered as a
api resource. Description of how to add more clients and resources will follow.

## Adding users

Right now doc-identity only allows an admin user to log on. The password has to be set as an env var (see Getting Started).
More support for user management will follow.

## Adding External Identity Providers:

### Google+ Identity

If you want to authenticate with google use create a app on the [console](https://console.developers.google.com/)

=> Create a project then add OAuth-Client-Id Access / Web App / use the http://localhost:<port> for authorized js sources
and http://localhost:<port>/signin-google as the authorization callback url

Also go to dashboard and activate Google+ Api

You can add more than one url. You should also add the urls for your hosted solution if you have done that. 

then add something like this to Startup.Configure:

            // middleware for google authentication
            app.UseGoogleAuthentication(new GoogleOptions
            {
                AuthenticationScheme = "Google",
                SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,
                ClientId = "<yourclientid>.apps.googleusercontent.com",
                ClientSecret = "<yourclientsecret>"
            });