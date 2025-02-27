#!/bin/bash

WORKSPACE=..
LUBAN_DLL=$WORKSPACE/Tools/Luban/Luban.dll
CONF_ROOT=.

dotnet $LUBAN_DLL \
    -t client \
    -c cs-bin \
    -d bin \
    --conf $CONF_ROOT/luban.conf \
    -x outputCodeDir=$WORKSPACE/Assets/Scripts/Gen/Luban \
    -x outputDataDir=$WORKSPACE/Assets/res/datas/bytes \
    -x pathValidator.rootDir=$WORKSPACE \
    -x l10n.provider=default \
    -x l10n.textFile.path=*@$WORKSPACE/DataTables/Datas/l10n/texts.json \
    -x l10n.textFile.keyFieldName=key
