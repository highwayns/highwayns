package com.highwayns.ht.imservice.event;

import com.highwayns.ht.ui.adapter.album.ImageItem;

import java.util.List;

/**
 * @author : yingmu on 15-1-16.
 * @email : yingmu@highwayns.com.
 */
public class SelectEvent {
    private List<ImageItem> list;
    public SelectEvent(List<ImageItem> list){
        this.list = list;
    }

    public List<ImageItem> getList() {
        return list;
    }

    public void setList(List<ImageItem> list) {
        this.list = list;
    }
}
