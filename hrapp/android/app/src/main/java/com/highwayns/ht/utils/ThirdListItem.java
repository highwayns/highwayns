package com.highwayns.ht.utils;

import android.graphics.drawable.Drawable;

/**
 * Created by highwayns on 2017/7/21.
 */
//ListItem资源类
public class ThirdListItem {
    private Drawable image;
    private String title;
    private String info;

    public Drawable getImage(){
        return image;
    }

    public void setImage(Drawable image) {
        this.image = image;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public void setInfo(String info) {
        this.info = info;
    }

    public String getInfo(){
        return info;
    }
}