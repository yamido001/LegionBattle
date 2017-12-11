简介：
文件存储在三个位置，服务器，客户端DataPath(persistentDataPath下的一个目录)(可读可写),客户端StreamPath(对于Ios和PC是streamingAssetsPath，对于Android是persistentDataPath中的一个目录)(只读)。
为了提高用户体验，如果StreamPath中文件存在，那么就不需要再从服务器下载。这样带来的影响有:
  ①读取文件时，逻辑模块调用FileMirrorMgr时只能传入文件名，在FileMirrorMgr内部找到相对路径(相对于存放可下载数据的根目录)然后再在DataPath中的这个相对目录下查找文件，
  如果没有再在StreamPath目录中查找文件。同时NewMirrorMgr不关心DataPath的实际位置，这个通过FileMgr来维护。
  ②服务器!=客户端DataPath，服务器!=客户端StreamPath不一定一致。服务器<=客户端DataPath+客户端StreamPath。这样DataPath就需要两个Md5值，一个是对应服务器的，一个对应本地FileList。
   发现LocalFileListMd5和FileList不一致时，代表文件被其他系统修改或者历史下载中断。

同时，启动时Android平台从streamingAssetsPath
向外部目录拷贝时，在目标目录存储版本号属性，拷贝成功后写入本包的版本号。下一次再拷贝时先读取版本号做比较，再决定是不是需要拷贝。
分别读取内外两个目录的文件列表，找到外部目录需要删除的文件(无用文件以及需要更新的文件)，进行删除后，跟新外部目录的文件列表（内存和硬盘）。再从内部目录向外拷贝，拷贝完成后Append一条信息。
本想沿用现有的拷贝逻辑，不进行修改，但是面临问题是，如果外部目录存不存在文件列表，拷贝时会把文件列表拷贝出去。但是文件列表中的文件如果在外部存在，就不会拷贝。导致文件列表和实际文件不一致，
并且不会被检查出来（因为不会本地进行md5校验）,所以需要一套新的拷贝流程

FileMirrorMgr的职责：
①.读取文件时，负责先在DataPath中查找，再在StreamPath中查找

下载模块
①.先读取DataPath目录下的LocalFileListMd5,ServerFileListMd5和FileList,再用FileList和实际文件进行双向对比，保证FileList和实际文件一致(防止被其他系统修改或者上传下载中断).
	如果不一致清除内存中LocalFileListMd5。然后获取FileList的MD5，和LocalFileListMd5进行比较，如果不一致清除ServerFileListMd5。按照顺序，依次把ServerFileListMd5,LocalFileListMd5
	FileList写入硬盘。这一步结束时，保证LocalFileListMd5和本地的FileList一致，而FileList和实际的文件一致。
②.用服务器的MD5值和ServerFileListMd5进行比较，一致则完成下载。不一致进行③
③.请求服务器的FileList,
④.用服务器的FileList先和DataPath下的FileList进行差分比较，先删除掉无用的资源。然后检查文件是否更新，如果更新删除硬盘文件，加入下载列表。如果Path改变，直接移动目录。如果不存在，再检查StreamPath
   是否存在，如果都                                                                                                                                                                                                                                     不存在，那么加入下载列表。
⑤.清除硬盘中的LocalFileListMd5，下载所有需要下载的文件，每下载完一个文件， 先把文件写入硬盘，再把这个文件的信息写入到硬盘的文件列表中（Append方式),并追加到内存中的FileList中
⑥.读取硬盘中的FileList,生成LocalFileListMd5。按照顺序把LocalFileListMd5和ServerFileListMd5依次写入硬盘

注意：
文件列表和Version文件都必须是无BOM的UTF8编码