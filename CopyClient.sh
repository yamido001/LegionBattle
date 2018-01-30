ClientPlugins=Client/proj_unity/Assets/Plugins/
cp Server/bin/Debug/LBCSCommon.dll ${ClientPlugins}LBCSCommon.dll

ClientPluginsMath=${ClientPlugins}LBMath

if [ ! -x "${ClientPluginsMath}" ]; then
	mkdir "${ClientPluginsMath}"
else
	rm -r ${ClientPluginsMath}/*
fi

CurPath=$(cd $(dirname $0); pwd)
ClientPluginsMath=${CurPath}/${ClientPluginsMath}

cd Server/LBMath
for file in `ls $1`       #注意此处这是两个反引号，表示运行系统命令
do
	if [ -f $file ];  #注意此处之间一定要加上空格，否则会报错
	then
		if [ "${file##*.}"x = "cs"x ];
		then
			cp $file ${ClientPluginsMath}/${file}
		fi
	fi
done