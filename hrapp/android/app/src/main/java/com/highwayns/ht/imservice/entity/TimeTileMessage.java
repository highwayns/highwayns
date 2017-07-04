package com.highwayns.ht.imservice.entity;

/**
 * @author : yingmu on 15-1-8.
 * @email : yingmu@highwayns.com.
 */
public class TimeTileMessage {
    private int time;
    public TimeTileMessage(int mTime){
        time= mTime;
    }

    public int getTime() {
        return time;
    }

    public void setTime(int time) {
        this.time = time;
    }
}
