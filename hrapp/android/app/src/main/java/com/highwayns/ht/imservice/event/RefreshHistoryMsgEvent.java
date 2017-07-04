package com.highwayns.ht.imservice.event;

import com.highwayns.ht.DB.entity.MessageEntity;

import java.util.List;

/**
 * @author : yingmu on 15-3-26.
 * @email : yingmu@highwayns.com.
 *
 * 异步刷新历史消息
 */
public class RefreshHistoryMsgEvent {
   public int pullTimes;
   public int lastMsgId;
   public int count;
   public List<MessageEntity> listMsg;
   public int peerId;
   public int peerType;
   public String sessionKey;

   public RefreshHistoryMsgEvent(){}

}
