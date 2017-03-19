<?php

/**
 * Created by JetBrains PhpStorm.
 * User: taoqili
 * Date: 12-7-18
 * Time: 涓婂崍11: 32
 * UEditor缂栬緫鍣ㄩ€氱敤涓婁紶绫? */
class Uploader
{
    private $fileField; //鏂囦欢鍩熷悕
    private $file; //鏂囦欢涓婁紶瀵硅薄
    private $base64; //鏂囦欢涓婁紶瀵硅薄
    private $config; //閰嶇疆淇℃伅
    private $oriName; //鍘熷鏂囦欢鍚?    private $fileName; //鏂版枃浠跺悕
    private $fullName; //瀹屾暣鏂囦欢鍚?鍗充粠褰撳墠閰嶇疆鐩綍寮€濮嬬殑URL
    private $filePath; //瀹屾暣鏂囦欢鍚?鍗充粠褰撳墠閰嶇疆鐩綍寮€濮嬬殑URL
    private $fileSize; //鏂囦欢澶у皬
    private $fileType; //鏂囦欢绫诲瀷
    private $stateInfo; //涓婁紶鐘舵€佷俊鎭?
    private $stateMap = array( //涓婁紶鐘舵€佹槧灏勮〃锛屽浗闄呭寲鐢ㄦ埛闇€鑰冭檻姝ゅ鏁版嵁鐨勫浗闄呭寲
        "SUCCESS", //涓婁紶鎴愬姛鏍囪锛屽湪UEditor涓唴涓嶅彲鏀瑰彉锛屽惁鍒檉lash鍒ゆ柇浼氬嚭閿?        "鏂囦欢澶у皬瓒呭嚭 upload_max_filesize 闄愬埗",
        "鏂囦欢澶у皬瓒呭嚭 MAX_FILE_SIZE 闄愬埗",
        "鏂囦欢鏈瀹屾暣涓婁紶",
        "娌℃湁鏂囦欢琚笂浼?,
        "涓婁紶鏂囦欢涓虹┖",
        "ERROR_TMP_FILE" => "涓存椂鏂囦欢閿欒",
        "ERROR_TMP_FILE_NOT_FOUND" => "鎵句笉鍒颁复鏃舵枃浠?,
        "ERROR_SIZE_EXCEED" => "鏂囦欢澶у皬瓒呭嚭缃戠珯闄愬埗",
        "ERROR_TYPE_NOT_ALLOWED" => "鏂囦欢绫诲瀷涓嶅厑璁?,
        "ERROR_CREATE_DIR" => "鐩綍鍒涘缓澶辫触",
        "ERROR_DIR_NOT_WRITEABLE" => "鐩綍娌℃湁鍐欐潈闄?,
        "ERROR_FILE_MOVE" => "鏂囦欢淇濆瓨鏃跺嚭閿?,
        "ERROR_FILE_NOT_FOUND" => "鎵句笉鍒颁笂浼犳枃浠?,
        "ERROR_WRITE_CONTENT" => "鍐欏叆鏂囦欢鍐呭閿欒",
        "ERROR_UNKNOWN" => "鏈煡閿欒",
        "ERROR_DEAD_LINK" => "閾炬帴涓嶅彲鐢?,
        "ERROR_HTTP_LINK" => "閾炬帴涓嶆槸http閾炬帴",
        "ERROR_HTTP_CONTENTTYPE" => "閾炬帴contentType涓嶆纭?
    );

    /**
     * 鏋勯€犲嚱鏁?     * @param string $fileField 琛ㄥ崟鍚嶇О
     * @param array $config 閰嶇疆椤?     * @param bool $base64 鏄惁瑙ｆ瀽base64缂栫爜锛屽彲鐪佺暐銆傝嫢寮€鍚紝鍒?fileField浠ｈ〃鐨勬槸base64缂栫爜鐨勫瓧绗︿覆琛ㄥ崟鍚?     */
    public function __construct($fileField, $config, $type = "upload")
    {
        $this->fileField = $fileField;
        $this->config = $config;
        $this->type = $type;
        if ($type == "remote") {
            $this->saveRemote();
        } else if($type == "base64") {
            $this->upBase64();
        } else {
            $this->upFile();
        }

        $this->stateMap['ERROR_TYPE_NOT_ALLOWED'] = iconv('unicode', 'gbk', $this->stateMap['ERROR_TYPE_NOT_ALLOWED']);
    }

    /**
     * 涓婁紶鏂囦欢鐨勪富澶勭悊鏂规硶
     * @return mixed
     */
    private function upFile()
    {
        $file = $this->file = $_FILES[$this->fileField];
        if (!$file) {
            $this->stateInfo = $this->getStateInfo("ERROR_FILE_NOT_FOUND");
            return;
        }
        if ($this->file['error']) {
            $this->stateInfo = $this->getStateInfo($file['error']);
            return;
        } else if (!file_exists($file['tmp_name'])) {
            $this->stateInfo = $this->getStateInfo("ERROR_TMP_FILE_NOT_FOUND");
            return;
        } else if (!is_uploaded_file($file['tmp_name'])) {
            $this->stateInfo = $this->getStateInfo("ERROR_TMPFILE");
            return;
        }

        $this->oriName = $file['name'];
        $this->fileSize = $file['size'];
        $this->fileType = $this->getFileExt();
        $this->fullName = $this->getFullName();
        $this->filePath = $this->getFilePath();
        $this->fileName = $this->getFileName();
        $dirname = dirname($this->filePath);

        //妫€鏌ユ枃浠跺ぇ灏忔槸鍚﹁秴鍑洪檺鍒?        if (!$this->checkSize()) {
            $this->stateInfo = $this->getStateInfo("ERROR_SIZE_EXCEED");
            return;
        }

        //妫€鏌ユ槸鍚︿笉鍏佽鐨勬枃浠舵牸寮?        if (!$this->checkType()) {
            $this->stateInfo = $this->getStateInfo("ERROR_TYPE_NOT_ALLOWED");
            return;
        }

        //鍒涘缓鐩綍澶辫触
        if (!file_exists($dirname) && !mkdir($dirname, 0777, true)) {
            $this->stateInfo = $this->getStateInfo("ERROR_CREATE_DIR");
            return;
        } else if (!is_writeable($dirname)) {
            $this->stateInfo = $this->getStateInfo("ERROR_DIR_NOT_WRITEABLE");
            return;
        }

        //绉诲姩鏂囦欢
        if (!(move_uploaded_file($file["tmp_name"], $this->filePath) && file_exists($this->filePath))) { //绉诲姩澶辫触
            $this->stateInfo = $this->getStateInfo("ERROR_FILE_MOVE");
        } else { //绉诲姩鎴愬姛
            $this->stateInfo = $this->stateMap[0];
        }
    }

    /**
     * 澶勭悊base64缂栫爜鐨勫浘鐗囦笂浼?     * @return mixed
     */
    private function upBase64()
    {
        $base64Data = $_POST[$this->fileField];
        $img = base64_decode($base64Data);

        $this->oriName = $this->config['oriName'];
        $this->fileSize = strlen($img);
        $this->fileType = $this->getFileExt();
        $this->fullName = $this->getFullName();
        $this->filePath = $this->getFilePath();
        $this->fileName = $this->getFileName();
        $dirname = dirname($this->filePath);

        //妫€鏌ユ枃浠跺ぇ灏忔槸鍚﹁秴鍑洪檺鍒?        if (!$this->checkSize()) {
            $this->stateInfo = $this->getStateInfo("ERROR_SIZE_EXCEED");
            return;
        }

        //鍒涘缓鐩綍澶辫触
        if (!file_exists($dirname) && !mkdir($dirname, 0777, true)) {
            $this->stateInfo = $this->getStateInfo("ERROR_CREATE_DIR");
            return;
        } else if (!is_writeable($dirname)) {
            $this->stateInfo = $this->getStateInfo("ERROR_DIR_NOT_WRITEABLE");
            return;
        }

        //绉诲姩鏂囦欢
        if (!(file_put_contents($this->filePath, $img) && file_exists($this->filePath))) { //绉诲姩澶辫触
            $this->stateInfo = $this->getStateInfo("ERROR_WRITE_CONTENT");
        } else { //绉诲姩鎴愬姛
            $this->stateInfo = $this->stateMap[0];
        }

    }

    /**
     * 鎷夊彇杩滅▼鍥剧墖
     * @return mixed
     */
    private function saveRemote()
    {
        $imgUrl = htmlspecialchars($this->fileField);
        $imgUrl = str_replace("&amp;", "&", $imgUrl);

        //http寮€澶撮獙璇?        if (strpos($imgUrl, "http") !== 0) {
            $this->stateInfo = $this->getStateInfo("ERROR_HTTP_LINK");
            return;
        }
        //鑾峰彇璇锋眰澶村苟妫€娴嬫閾?        $heads = get_headers($imgUrl);
        if (!(stristr($heads[0], "200") && stristr($heads[0], "OK"))) {
            $this->stateInfo = $this->getStateInfo("ERROR_DEAD_LINK");
            return;
        }
        //鏍煎紡楠岃瘉(鎵╁睍鍚嶉獙璇佸拰Content-Type楠岃瘉)
        $fileType = strtolower(strrchr($imgUrl, '.'));
        if (!in_array($fileType, $this->config['allowFiles']) || stristr($heads['Content-Type'], "image")) {
            $this->stateInfo = $this->getStateInfo("ERROR_HTTP_CONTENTTYPE");
            return;
        }

        //鎵撳紑杈撳嚭缂撳啿鍖哄苟鑾峰彇杩滅▼鍥剧墖
        ob_start();
        $context = stream_context_create(
            array('http' => array(
                'follow_location' => false // don't follow redirects
            ))
        );
        readfile($imgUrl, false, $context);
        $img = ob_get_contents();
        ob_end_clean();
        preg_match("/[\/]([^\/]*)[\.]?[^\.\/]*$/", $imgUrl, $m);

        $this->oriName = $m ? $m[1]:"";
        $this->fileSize = strlen($img);
        $this->fileType = $this->getFileExt();
        $this->fullName = $this->getFullName();
        $this->filePath = $this->getFilePath();
        $this->fileName = $this->getFileName();
        $dirname = dirname($this->filePath);

        //妫€鏌ユ枃浠跺ぇ灏忔槸鍚﹁秴鍑洪檺鍒?        if (!$this->checkSize()) {
            $this->stateInfo = $this->getStateInfo("ERROR_SIZE_EXCEED");
            return;
        }

        //鍒涘缓鐩綍澶辫触
        if (!file_exists($dirname) && !mkdir($dirname, 0777, true)) {
            $this->stateInfo = $this->getStateInfo("ERROR_CREATE_DIR");
            return;
        } else if (!is_writeable($dirname)) {
            $this->stateInfo = $this->getStateInfo("ERROR_DIR_NOT_WRITEABLE");
            return;
        }

        //绉诲姩鏂囦欢
        if (!(file_put_contents($this->filePath, $img) && file_exists($this->filePath))) { //绉诲姩澶辫触
            $this->stateInfo = $this->getStateInfo("ERROR_WRITE_CONTENT");
        } else { //绉诲姩鎴愬姛
            $this->stateInfo = $this->stateMap[0];
        }

    }

    /**
     * 涓婁紶閿欒妫€鏌?     * @param $errCode
     * @return string
     */
    private function getStateInfo($errCode)
    {
        return !$this->stateMap[$errCode] ? $this->stateMap["ERROR_UNKNOWN"] : $this->stateMap[$errCode];
    }

    /**
     * 鑾峰彇鏂囦欢鎵╁睍鍚?     * @return string
     */
    private function getFileExt()
    {
        return strtolower(strrchr($this->oriName, '.'));
    }

    /**
     * 閲嶅懡鍚嶆枃浠?     * @return string
     */
    private function getFullName()
    {
        //鏇挎崲鏃ユ湡浜嬩欢
        $t = time();
        $d = explode('-', date("Y-y-m-d-H-i-s"));
        $format = $this->config["pathFormat"];
        $format = str_replace("{yyyy}", $d[0], $format);
        $format = str_replace("{yy}", $d[1], $format);
        $format = str_replace("{mm}", $d[2], $format);
        $format = str_replace("{dd}", $d[3], $format);
        $format = str_replace("{hh}", $d[4], $format);
        $format = str_replace("{ii}", $d[5], $format);
        $format = str_replace("{ss}", $d[6], $format);
        $format = str_replace("{time}", $t, $format);

        //杩囨护鏂囦欢鍚嶇殑闈炴硶鑷礋,骞舵浛鎹㈡枃浠跺悕
        $oriName = substr($this->oriName, 0, strrpos($this->oriName, '.'));
        $oriName = preg_replace("/[\|\?\"\<\>\/\*\\\\]+/", '', $oriName);
        $format = str_replace("{filename}", $oriName, $format);

        //鏇挎崲闅忔満瀛楃涓?        $randNum = rand(1, 10000000000) . rand(1, 10000000000);
        if (preg_match("/\{rand\:([\d]*)\}/i", $format, $matches)) {
            $format = preg_replace("/\{rand\:[\d]*\}/i", substr($randNum, 0, $matches[1]), $format);
        }

        $ext = $this->getFileExt();
        return $format . $ext;
    }

    /**
     * 鑾峰彇鏂囦欢鍚?     * @return string
     */
    private function getFileName () {
        return substr($this->filePath, strrpos($this->filePath, '/') + 1);
    }

    /**
     * 鑾峰彇鏂囦欢瀹屾暣璺緞
     * @return string
     */
    private function getFilePath()
    {
        $fullname = $this->fullName;
        $rootPath = $_SERVER['DOCUMENT_ROOT'];

        if (substr($fullname, 0, 1) != '/') {
            $fullname = '/' . $fullname;
        }

        return $rootPath . $fullname;
    }

    /**
     * 鏂囦欢绫诲瀷妫€娴?     * @return bool
     */
    private function checkType()
    {
        return in_array($this->getFileExt(), $this->config["allowFiles"]);
    }

    /**
     * 鏂囦欢澶у皬妫€娴?     * @return bool
     */
    private function  checkSize()
    {
        return $this->fileSize <= ($this->config["maxSize"]);
    }

    /**
     * 鑾峰彇褰撳墠涓婁紶鎴愬姛鏂囦欢鐨勫悇椤逛俊鎭?     * @return array
     */
    public function getFileInfo()
    {
        return array(
            "state" => $this->stateInfo,
            "url" => $this->fullName,
            "title" => $this->fileName,
            "original" => $this->oriName,
            "type" => $this->fileType,
            "size" => $this->fileSize
        );
    }

}