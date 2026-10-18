#!/usr/bin/env bash
# usage: tools/gitbatch.sh commits.txt   (each line: path<TAB>message)
# Commits each path (plus its .meta) separately, then pushes once. See Docs 09 §9 (Farm Mode).
set -e
while IFS=$'\t' read -r p m; do
  [ -z "$p" ] && continue
  git add -- "$p"
  if [ -e "$p.meta" ]; then git add -- "$p.meta"; fi
  git diff --cached --quiet || git commit -q -m "$m"
done < "$1"
git push -q
