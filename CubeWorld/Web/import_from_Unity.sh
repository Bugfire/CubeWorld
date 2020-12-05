#!/bin/bash

# Unity WebGL Build
rm -rf public/unity
cp -R ../BuildWebGL/Build public/unity
npx js-beautify -s 2 < ../BuildWebGL/Build/UnityLoader.js > public/unity/UnityLoader.js
patch -p0 < patches/UnityLoader.js.diff

# License.txt
cp ../Assets/License.txt src/assets/License.md
