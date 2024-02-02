[![EO principles respected here](http://www.elegantobjects.org/badge.svg)](http://www.elegantobjects.org)

Responsible: ETO

# Metafile Template Repo
> Collection of metafiles useful for many ICARUS projects

Metafiles are files like the gitignore, Issue templates or build configs which aren't project specific and are commonly used in many repos.


## Usage

These files can be copied over to your new or existing project.
If you adapt these files in your project consider creating an Issue here to improve them for everyone.


## Configuration Rules

In general, it is best to use defaults and try to use as little extra configuration and metafiles as possible.
If that doesn't work the configuration or at least the filenames should be the same for the same thing in every repo.
If your project is deviating from the common configuration or uses extensively large and/or complex configuration your project will be hard to maintain for someone else.
This is something we want to avoid.


## Files

A brief introduction and where to find more documentation about files in this repo.

If you seek files which are specific to the C# project seek the [coding-guidelines](https://github.com/icarus-consulting/coding-guidelines#github-repository-structure).
This repo tries to be project independent for simple "copy over and done" files while the coding-guidelines are also describing project specifics.
Feel free to suggest improvements.

### .github Folder

Files in this folder are evaluated by Github.

The `ISSUE_TEMPLATE` folder contains Markdown (`*.md`) files which can be selected as a template when creating a new [Issue](https://help.github.com/articles/github-glossary/#issue).

The `PULL_REQUEST_TEMPLATE.md` provides a template which is used for [Pull Requests (PR)](https://help.github.com/articles/github-glossary/#pull-request).

The `dependabot.yml` provides a configuration on which and how the Dependabot should check for Dependencies which can be updated.
Keep in mind that the Dependabot has two variations: ["Dependabot version updates"](https://docs.github.com/en/code-security/supply-chain-security/keeping-your-dependencies-updated-automatically/about-dependabot-version-updates) and ["Dependabot security updates"](https://docs.github.com/en/code-security/supply-chain-security/managing-vulnerabilities-in-your-projects-dependencies/about-dependabot-security-updates).
The `dependabot.yml` configures the "version updates" and not the "security updates".
The security updates are only "on" or "off" and are configured in the repository settings.

#### workflows folder

`.github/workflows/*.yml` files are workflows which are run via GitHub Actions.
They can be used for automation of GitHub Tasks (handling issues, PRs, …) and for reacting on code changes (continous testing, integration, …).
Actions in public repos are free while private repos cost per runtime (minute).
(Windows Runners are double the cost of Linux Runners, macOS Runners 10 times the cost)

One workflow described in one file can consist of multiple jobs.
Each job contains multiple steps.
A step might be something like `git pull` or `npm test`.
Steps can be run in a command line or use an "action" which are basically custom programs created by other people.
Keep in mind that these actions are able to access our private code and need to be checked before they are used!
They are able to steal code or secrets!

For detailed info check the [Official documentation](https://docs.github.com/en/actions/learn-github-actions/introduction-to-github-actions) and the [workflow syntax reference](https://docs.github.com/en/actions/reference/workflow-syntax-for-github-actions).

### .editorconfig

EditorConfig helps maintain consistent coding styles for multiple developers working on the same project across various editors and IDEs.
The EditorConfig project consists of **a file format** for defining coding styles and a collection of **text editor plugins** that enable editors to read the file format and adhere to defined styles.
EditorConfig files are easily readable and they work nicely with version control systems.

[EditorConfig documentation](https://editorconfig.org/)

### .gitignore
The `.gitignore` file specifies intentionally untracked files that Git should ignore.
Files already tracked by Git are not affected.
Use the `.gitignore` in this repository which considers cake and Visual Studio files or create a new one via www.gitignore.io.

[gitignore documentation](https://git-scm.com/docs/gitignore)

### appveyor.yml

Settings for the AppVeyor Continuous Integration.
This probably requires a C# specific `build.ps1` which is not included in this repo as it is specific to project and repository names.

[appveyor.yml reference](https://www.appveyor.com/docs/appveyor-yml/)

### README.md

This file is the first file most people see of a project.
It should introduce to someone why they would want this repo.
Its followed by an in depth description about repo specifics and how to get started with this specific repo.
