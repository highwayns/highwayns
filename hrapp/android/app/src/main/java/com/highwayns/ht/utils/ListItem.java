package com.highwayns.ht.utils;

import android.graphics.drawable.Drawable;

/**
 * Created by highwayns on 2017/7/21.
 */
//ListItem资源类
public class ListItem {
    private Drawable image;
    private String title;

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
}
