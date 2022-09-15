#!/bin/bash
# git script to easily create and manage the branching strategy

# create branch
# sh git.sh branch <name>

# rebase branch
# sh git.sh rebase

# create pr url
# check progress, does it need a rebase?
# get urls to create prs for all the environments
# sh git.sh pr

# pull main
# sh git.sh pull-main

usage="Usage:
$(basename $0) help   see this message again
$(basename $0) branch <name>    create a branch with <name> from origin main
$(basename $0) pr [--no-checks]   pushes any changes on the current branch then open the urls to create all prs
    --no-checks   disables all prechecks and only pushes then returns the urls
$(basename $0) rebase   checks if the current branch needs to be rebased off main and if so does it
$(basename $0) pull-main   updates local main with remote main
"

repo_name=$(basename -s .git `git config --get remote.origin.url`)
branch_name=$(git rev-parse --abbrev-ref HEAD)
gh_host='github.com'

# ----------------
# Functions
# ----------------
err_if_uncommitted_changes () {
  # if there are uncommitted or unstashed changes, exit with status 1
  if ! git diff-index --quiet HEAD; then
    echo "Uncommitted or unstashed changes detected, aborting"
    git status # print changes to stdout
    exit 1
  fi
}

update_main () {
  # ff main to origin/main, switches back to original branch
  git fetch
  err_if_uncommitted_changes
  git checkout main
  git merge origin/main --ff-only
  git checkout "$branch_name"
}

no_rebase_needed () {
  # 1 if main tip is ancestor of current branch HEAD
  git fetch
  git merge-base --is-ancestor origin/main HEAD
}


# Script
if [[ $1 == 'help' || $# -eq 0 ]]; then
  # print help text
  echo "$usage"
  exit 0
fi

if [[ $1 == 'branch' ]]; then
  # create a branch
  # - update main
  # - create branch off main
  # - setup upstream
  update_main
  git checkout main
  git checkout -b $2
  git push -u origin HEAD
elif [[ $1 == 'pr' ]]; then
  # return urls to create prs for this feature
  # - check if a rebase is needed
  # - return urls
  skip_checks=0
  if [[ -n "$2" ]]; then
    if [[ $2 != '--no-checks' ]]; then
      echo "Unknown argument: $2"
      exit 1
    fi
    skip_checks=1
  fi
  if [[ $skip_checks -ne 1 ]]; then
    if ! no_rebase_needed; then
      echo "Rebase needed. $branch_name is not up to date with remote main"
      exit 1
    fi
    err_if_uncommitted_changes
  fi
  git push
  base_url='https://github.com/SanDiegoCountySheriff'
  url="$base_url/$repo_name/compare"
  echo "main: $url/main...$branch_name?expand=1&labels=main"
elif [[ $1 == 'rebase' ]]; then
  # rebases the current branch
  # - check if rebase necessary
  # - does it if so
  if [[ $$branch_name == 'main' ]]; then
    update_main
    exit 0
  fi
  if no_rebase_needed; then
    echo "No rebase needed. $branch_name is up to date with remote main"
    exit 0
  fi
  echo "Rebasing $branch_name"
  err_if_uncommitted_changes
  update_main
  git rebase main
  git pull
elif [[ $1 == 'pull-main' ]]; then
  update_main
else
  echo "Unknown arguments:" "$@"
  echo "$usage"
  exit 1
fi
