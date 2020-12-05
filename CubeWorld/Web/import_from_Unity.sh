#!/bin/bash

# Unity WebGL Build
rm -rf public/unity
cp -R ../BuildWebGL/Build public/unity

LOADER_FILENAME=`basename ../BuildWebGL/Build/*.js`
npx js-beautify -s 2 < ../BuildWebGL/Build/$LOADER_FILENAME > public/unity/UnityLoader.js
patch -p0 < patches/UnityLoader.js.diff
rm public/unity/$LOADER_FILENAME

JSON_FILENAME=`basename ../BuildWebGL/Build/*.json`
echo "export const JSON_FILENAME = \"$JSON_FILENAME\";" > src/unity_settings.ts

# License.txt
cp ../Assets/License.txt src/assets/License.md
