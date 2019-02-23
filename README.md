# Github_Followers

**Github_Followers** is a simple web API designed to show the followers of github users. The API not only returns the followers of a specified user, but also their followers followers up to four layers deep or a total of 100 followers, which ever comes first.

## Using Github_Followers

Using **Github_Followers** is simple, all you need to do is clone this repository and install the .Net Core run-time version 2.2. You can find the run-time [here](https://dotnet.microsoft.com/download/dotnet-core/2.2).

Once you have installed the .Net core run-time, open your command prompt and navigate to the location that you saved this repository. Once you are here type the command 
> dotnet GithubFollowers.dll

into your command prompt and hit enter, this will start the server on local port 5000.

Once the server is running if you want to make a call to the API all you need to do is copy the following into the address bar of your favorite browser (but maybe not Edge):
> http://localhost:5000/github/followers/:username

Where :username is the username of the Github user you are interested in.