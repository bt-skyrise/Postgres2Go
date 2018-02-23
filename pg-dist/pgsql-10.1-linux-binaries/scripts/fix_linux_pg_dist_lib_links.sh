#!/bin/bash

declare -A PG_LIB_LINKS

PG_LIB_LINKS=(
 ["libpq.so"]="libpq.so.5.10" \
 ["libpq.so.5"]="libpq.so.5.10" \
 ["libpgtypes.so"]="libpgtypes.so.3.10" \
 ["libpgtypes.so.3"]="libpgtypes.so.3.10" \
 ["libicuuc.so"]="libicuuc.so.53.1" \
 ["libicuuc.so.53"]="libicuuc.so.53.1" \
 ["libicutu.so"]="libicutu.so.53.1" \
 ["libicutu.so.53"]="libicutu.so.53.1" \
 ["libicutest.so"]="libicutest.so.53.1" \
 ["libicutest.so.53"]="libicutest.so.53.1" \
 ["libiculx.so"]="libiculx.so.53.1" \
 ["libiculx.so.53"]="libiculx.so.53.1" \
 ["libicule.so"]="libicule.so.53.1" \
 ["libicule.so.53"]="libicule.so.53.1" \
 ["libicuio.so"]="libicuio.so.53.1" \
 ["libicuio.so.53"]="libicuio.so.53.1" \
 ["libicui18n.so"]="libicui18n.so.53.1" \
 ["libicui18n.so.53"]="libicui18n.so.53.1" \
 ["libicudata.so"]="libicudata.so.53.1" \
 ["libicudata.so.53"]="libicudata.so.53.1" \
 ["libecpg.so"]="libecpg.so.6.10" \
 ["libecpg.so.6"]="libecpg.so.6.10" \
 ["libecpg_compat.so"]="libecpg_compat.so.3.10" \
 ["libecpg_compat.so.3"]="libecpg_compat.so.3.10" \
 ["libcurl.so"]="libcurl.so.4.4.0" \
 ["libcurl.so.4"]="libcurl.so.4.4.0" \
)

PG_BINARIES="$1"

cd $PG_BINARIES
cd lib

echo "Changed working_dir to $PWD"

for link in ${!PG_LIB_LINKS[@]};
do
	if [[ -e $link ]] || [[ -h $link ]]
	then 
		echo "Removing $link"
		rm $link
	else
		echo "Not found $link"
	fi
	
	target="${PG_LIB_LINKS[$link]}"
	echo "Create link $link targeting $target"
	ln -s $target $link
done
