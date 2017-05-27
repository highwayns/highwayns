<?php

// Calculate percentage between width and height
function tpl_function_resize_percent($maximum, $current)
{
	return (real)(100 * ($maximum / $current));
}

function tpl_function_resize_unpercent($percent, $whole)
{
	return (real)(($percent * $whole) / 100);
}

function tpl_function_resize_image($params, &$tpl)
{
	extract($params);

	if (empty($directory))
	{
		$tpl->trigger_error("resize_image: missing 'directory' parameter");
		return;
	}

	if (empty($thumbdir))
	{
		$thumbdir = $directory;
	}

	if (empty($filename))
	{
		$tpl->trigger_error("resize_image: missing 'filename' parameter");
		return;
	}

	if (empty($xscale))
	{
		$xscale = 2000;
	}
	$maximagewidth=$xscale;

	if (empty($xscale))
	{
		$yscale = 2000;
	}
	$maximageheight=$yscale;

	if (empty($alt))
	{
		$alt = "image";
	}

	if (empty($border))
	{
		$border = 0;
	}

	if (empty($daystokeep))
	{
		$daystokeep = 5;
	}

	if (!function_exists('gd_info'))
	{
		$tpl->trigger_error("resize_image: the GD library is not installed");
		return;
	}

	if(!file_exists($directory . $filename) && !empty($url) && function_exists('curl_init'))
	{
		$ch = curl_init ($url . $filename);
		$fp = fopen ($directory . $filename, "w");
		curl_setopt ($ch, CURLOPT_FILE, $fp);
		curl_setopt ($ch, CURLOPT_HEADER, 0);
		curl_exec ($ch);
		curl_close ($ch);
		fclose ($fp);
	}

	if(file_exists($directory . $filename))
	{
		$imageinfo = @getimagesize($directory . $filename);
		if(empty($imageinfo))
		{
			return;
		}

		if ($returntype == 1)
		{
			$imagewidth = $imageinfo[0];
			$imageheight = $imageinfo[1];

			if($maximagewidth < $imagewidth) {
				$imageheight = tpl_function_resize_unpercent(tpl_function_resize_percent($maximagewidth, $imagewidth), $imageheight);
				$imagewidth = $maximagewidth;
			}

			if($maximageheight < $imageheight) {
				$imagewidth = tpl_function_resize_unpercent(tpl_function_resize_percent($maximageheight, $imageheight), $imagewidth);
				$imageheight = $maximageheight;
			}
			return "<img " . $class . " src=\"" . $img_src . $filename . "\" width=\"" . $imagewidth . "\" height=\"" . $imageheight . "\" alt=\"" . $alt . "\" border=\"" . $border . "\">";
		}

		if (empty($thumbname))
		{
			$tpl->trigger_error("resize_image: missing 'thumbname' parameter");
			return;
		}

		$now=urlencode(date("F j, Y, g:i a"));

		$newimagepath = $thumbdir . $thumbname . $filename;

		$newthumbnail = 0;
		if(!file_exists($newimagepath))
		{
			copy($directory . $filename, $newimagepath);
			$newthumbnail = 1;
		}

		$datechanged = date("j", time()) - date("j", filemtime($newimagepath));
		if(($datechanged > -$daystokeep && $datechanged < $daystokeep) && $newthumbnail = 0)
		{
			// Do not rebuild
			$imagewidth = $imageinfo[0];
			$imageheight = $imageinfo[1];

			if($maximagewidth < $imagewidth)
			{
				$imageheight = tpl_function_resize_unpercent(tpl_function_resize_percent($maximagewidth, $imagewidth), $imageheight);
				$imagewidth = $maximagewidth;
			}

			if($maximageheight < $imageheight)
			{
				$imagewidth = tpl_function_resize_unpercent(tpl_function_resize_percent($maximageheight, $imageheight), $imagewidth);
				$imageheight = $maximageheight;
			}
		}
		else
		{
			// rebuild
			copy($directory . $filename, $newimagepath);

			$imagewidth = $imageinfo[0];
			$imageheight = $imageinfo[1];

			if($maximagewidth < $imagewidth)
			{
				$imageheight = tpl_function_resize_unpercent(tpl_function_resize_percent($maximagewidth, $imagewidth), $imageheight);
				$imagewidth = $maximagewidth;
			}

			if($maximageheight < $imageheight)
			{
				$imagewidth = tpl_function_resize_unpercent(tpl_function_resize_percent($maximageheight, $imageheight), $imagewidth);
				$imageheight = $maximageheight;
			}
			$imagewidth = round($imagewidth);
			$imageheight = round($imageheight);
			$scale = $imagewidth . "x" . $imageheight . "!";

			if (empty($binpath))
			{
				if($imageinfo[2] == 1)
				{
					$sourceimage = imagecreatefromgif($directory . $filename);
				}
				elseif($imageinfo[2] == 2)
				{
					$sourceimage = imagecreatefromjpeg($directory . $filename);
				}
				elseif($imageinfo[2] == 3)
				{
					$sourceimage = imagecreatefrompng($directory . $filename);
				}

				$destinationimage = imagecreatetruecolor($imagewidth, $imageheight);
				imagecopyresized($destinationimage, $sourceimage, 0, 0, 0, 0, $imagewidth, $imageheight, $imageinfo[0], $imageinfo[1]);
				if($imageinfo[2] == 1)
				{
					imagegif($destinationimage, $newimagepath);
				}
				elseif($imageinfo[2] == 2)
				{
					imageJPEG($destinationimage, $newimagepath, 75);
				}
				elseif($imageinfo[2] == 3)
				{
					imagepng($destinationimage, $newimagepath);
				}
				imagedestroy($sourceimage);
				imagedestroy($destinationimage);
			}
			else
			{
				if ($imageinfo[2] == 2)
				{
					system( $binpath . "mogrify  -quality 75 -geometry $scale $newimagepath");
				}
				else
				{
					system( $binpath . "mogrify  -geometry $scale $newimagepath");
				}
			}
		}

		return "<img " . $class . " src=\"" . $img_src . $thumbname . $filename . "?" . $now . "\" width=\"" . $imagewidth . "\" height=\"" . $imageheight . "\" alt=\"" . $alt . "\" border=\"" . $border . "\">";
	}
}
?>
