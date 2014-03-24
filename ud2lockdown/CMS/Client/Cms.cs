﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="ICMS")]
public interface ICMS
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMS/Customer", ReplyAction="http://tempuri.org/ICMS/CustomerResponse")]
    string Customer(string id);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICMS/Customer", ReplyAction="http://tempuri.org/ICMS/CustomerResponse")]
    System.Threading.Tasks.Task<string> CustomerAsync(string id);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface ICMSChannel : ICMS, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class CMSClient : System.ServiceModel.ClientBase<ICMS>, ICMS
{
    
    public CMSClient()
    {
    }
    
    public CMSClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public CMSClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public CMSClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public CMSClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public string Customer(string id)
    {
        return base.Channel.Customer(id);
    }
    
    public System.Threading.Tasks.Task<string> CustomerAsync(string id)
    {
        return base.Channel.CustomerAsync(id);
    }
}
