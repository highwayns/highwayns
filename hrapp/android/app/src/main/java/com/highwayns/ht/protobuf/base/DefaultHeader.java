package com.highwayns.ht.protobuf.base;

import com.highwayns.ht.config.SysConstant;
import com.highwayns.ht.imservice.support.SequenceNumberMaker;
import com.highwayns.ht.utils.Logger;

public class DefaultHeader extends Header {
    private Logger logger = Logger.getLogger(DefaultHeader.class);

    public DefaultHeader(int serviceId, int commandId) {
        setVersion((short) SysConstant.PROTOCOL_VERSION);
        setFlag((short) SysConstant.PROTOCOL_FLAG);
        setServiceId((short)serviceId);
        setCommandId((short)commandId);
        short seqNo = SequenceNumberMaker.getInstance().make();
        setSeqnum(seqNo);
        setReserved((short)SysConstant.PROTOCOL_RESERVED);

        logger.d("packet#construct Default Header -> serviceId:%d, commandId:%d, seqNo:%d", serviceId, commandId, seqNo);
    }
}
