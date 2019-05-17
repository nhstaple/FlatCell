# [Git Flow](https://github.com/nvie/gitflow) #

## [Install instructions.](https://github.com/nvie/gitflow/wiki/Installation]) ##

## Setup instructions. ##
```
git clone https://github.com/nhstaple/ECS189L-Group-Project/
cd ECS189L-Group-Project
git branch dev
git checkout dev
git pull origin dev
git flow init
:: Enter "master" for production release, and "dev" for "next release."
git flow feature start <your feature>
git flow feature publish <your feature>
:: Make changes and commit locally. To push-
git push origin feature/<your feature>
:: To finsh your feature and updated your changes to the dev branch so everyone can see it-
git flow feature finish <your feature>
:: Enter a useful message sumarizing your commits.
```

## Why use git flow? ##

So we can all work on indivudal components in our own `feature/` branches. Then we push to `dev` when we've finished our component.

After we have all finished a stage we create a `release/` branch to get rid of all of the bugs, add documentation, etc. Once that is finished we publish the `release/` branch and the `master` branch gets updated. Nick will make a tag so we can keep track of versions by downloading .zip files.

## [Visual guide to the git flow workflow.](https://danielkummer.github.io/git-flow-cheatsheet/) ##

# Explanation of the branches. #

### `dev` ###
This is the "work in progress" branch. We can directly push to it (not recoommended), or we can use `git flow feature` to create a `feature/` branch that will be forked from `dev.`

### `feature/` ###
This can be thought of an indivial module. Each of us can be assigned a task that's independent of one another, then we all work on our own `feature/` branch. This is forked from `dev` then updated to `dev` when completed.

##### Creation #####
`git flow feature start <your feature name>`

##### Pushing to github so others can view your code. #####
`git flow feature publish <your feature name>`

To update your commits-

`git push origin feature/<your feature name>`

##### To finish your feature and add it to `dev`. #####
`git flow feature finish <your feature name>`

### `release/` ###
I recommend that we create a `release/` branch for each stage we finish so we can back track our code/editor settings between functioning sections of code.

##### Making a `release/` branch. #####
`git flow release start <version number>`

`...`

`Fix bugs, add documentation, etc.`

##### Pushing to github. #####
`git flow release publish <version number>`

To update your commits-

`git push origin release/<version number>`

##### To finish a `release/` and update the changes to `master` and `dev`. #####
`git flow release finish <version number>`

### `hotfix/` ###
Exactly what it sounds like. This gets forked from `master`, then gets updated to `master` and `dev`.
