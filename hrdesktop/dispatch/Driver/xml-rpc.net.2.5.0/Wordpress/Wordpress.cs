using System;
using CookComputing.XmlRpc;

public struct StateStructRequest
{
  public int state1;  
  public int state2;
  public int state3;
}

[XmlRpcUrl("http://localhost/highwayns/xmlrpc.php")]
public interface IWordpress : IXmlRpcProxy
{
  [XmlRpcMethod("examples.getStateName")]
  string GetStateName(int stateNumber); 

  [XmlRpcMethod("examples.getStateStruct")]
  string GetStateNames(StateStructRequest request);
}

