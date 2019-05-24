﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Relational.Core.Client.RelationalProxy {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RelationalProxy.IRelationalService")]
    public interface IRelationalService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/DownloadReport", ReplyAction="http://tempuri.org/IRelationalService/DownloadReportResponse")]
        byte[] DownloadReport(string sql, string Connect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/DownloadReport", ReplyAction="http://tempuri.org/IRelationalService/DownloadReportResponse")]
        System.Threading.Tasks.Task<byte[]> DownloadReportAsync(string sql, string Connect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/DownloadReport2", ReplyAction="http://tempuri.org/IRelationalService/DownloadReport2Response")]
        byte[] DownloadReport2(string Connect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/DownloadReport2", ReplyAction="http://tempuri.org/IRelationalService/DownloadReport2Response")]
        System.Threading.Tasks.Task<byte[]> DownloadReport2Async(string Connect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/TestConnection", ReplyAction="http://tempuri.org/IRelationalService/TestConnectionResponse")]
        int TestConnection(string Connect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/TestConnection", ReplyAction="http://tempuri.org/IRelationalService/TestConnectionResponse")]
        System.Threading.Tasks.Task<int> TestConnectionAsync(string Connect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/GetData", ReplyAction="http://tempuri.org/IRelationalService/GetDataResponse")]
        string GetData(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/GetData", ReplyAction="http://tempuri.org/IRelationalService/GetDataResponse")]
        System.Threading.Tasks.Task<string> GetDataAsync(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/GetDataSetInternal1", ReplyAction="http://tempuri.org/IRelationalService/GetDataSetInternal1Response")]
        System.Data.DataSet GetDataSetInternal1(string sql, string Connect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/GetDataSetInternal1", ReplyAction="http://tempuri.org/IRelationalService/GetDataSetInternal1Response")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetDataSetInternal1Async(string sql, string Connect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/GetDataSetInternal2", ReplyAction="http://tempuri.org/IRelationalService/GetDataSetInternal2Response")]
        System.Data.DataSet GetDataSetInternal2(string sql, string[] sqlPar, string Connect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/GetDataSetInternal2", ReplyAction="http://tempuri.org/IRelationalService/GetDataSetInternal2Response")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetDataSetInternal2Async(string sql, string[] sqlPar, string Connect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/GetDataSetInternal3", ReplyAction="http://tempuri.org/IRelationalService/GetDataSetInternal3Response")]
        System.Data.DataSet GetDataSetInternal3(string sql, string[][] listParameters, string Connect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/GetDataSetInternal3", ReplyAction="http://tempuri.org/IRelationalService/GetDataSetInternal3Response")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetDataSetInternal3Async(string sql, string[][] listParameters, string Connect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/GetReportDataPaged", ReplyAction="http://tempuri.org/IRelationalService/GetReportDataPagedResponse")]
        System.Data.DataSet GetReportDataPaged(string[][] relReport, int from, int to, string Connect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/GetReportDataPaged", ReplyAction="http://tempuri.org/IRelationalService/GetReportDataPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetReportDataPagedAsync(string[][] relReport, int from, int to, string Connect);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/GetDataSetList", ReplyAction="http://tempuri.org/IRelationalService/GetDataSetListResponse")]
        System.Data.DataSet GetDataSetList(string[][] sqlParList);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelationalService/GetDataSetList", ReplyAction="http://tempuri.org/IRelationalService/GetDataSetListResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetDataSetListAsync(string[][] sqlParList);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IRelationalServiceChannel : Relational.Core.Client.RelationalProxy.IRelationalService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RelationalServiceClient : System.ServiceModel.ClientBase<Relational.Core.Client.RelationalProxy.IRelationalService>, Relational.Core.Client.RelationalProxy.IRelationalService {
        
        public RelationalServiceClient() {
        }
        
        public RelationalServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public RelationalServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RelationalServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RelationalServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public byte[] DownloadReport(string sql, string Connect) {
            return base.Channel.DownloadReport(sql, Connect);
        }
        
        public System.Threading.Tasks.Task<byte[]> DownloadReportAsync(string sql, string Connect) {
            return base.Channel.DownloadReportAsync(sql, Connect);
        }
        
        public byte[] DownloadReport2(string Connect) {
            return base.Channel.DownloadReport2(Connect);
        }
        
        public System.Threading.Tasks.Task<byte[]> DownloadReport2Async(string Connect) {
            return base.Channel.DownloadReport2Async(Connect);
        }
        
        public int TestConnection(string Connect) {
            return base.Channel.TestConnection(Connect);
        }
        
        public System.Threading.Tasks.Task<int> TestConnectionAsync(string Connect) {
            return base.Channel.TestConnectionAsync(Connect);
        }
        
        public string GetData(int value) {
            return base.Channel.GetData(value);
        }
        
        public System.Threading.Tasks.Task<string> GetDataAsync(int value) {
            return base.Channel.GetDataAsync(value);
        }
        
        public System.Data.DataSet GetDataSetInternal1(string sql, string Connect) {
            return base.Channel.GetDataSetInternal1(sql, Connect);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetDataSetInternal1Async(string sql, string Connect) {
            return base.Channel.GetDataSetInternal1Async(sql, Connect);
        }
        
        public System.Data.DataSet GetDataSetInternal2(string sql, string[] sqlPar, string Connect) {
            return base.Channel.GetDataSetInternal2(sql, sqlPar, Connect);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetDataSetInternal2Async(string sql, string[] sqlPar, string Connect) {
            return base.Channel.GetDataSetInternal2Async(sql, sqlPar, Connect);
        }
        
        public System.Data.DataSet GetDataSetInternal3(string sql, string[][] listParameters, string Connect) {
            return base.Channel.GetDataSetInternal3(sql, listParameters, Connect);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetDataSetInternal3Async(string sql, string[][] listParameters, string Connect) {
            return base.Channel.GetDataSetInternal3Async(sql, listParameters, Connect);
        }
        
        public System.Data.DataSet GetReportDataPaged(string[][] relReport, int from, int to, string Connect) {
            return base.Channel.GetReportDataPaged(relReport, from, to, Connect);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetReportDataPagedAsync(string[][] relReport, int from, int to, string Connect) {
            return base.Channel.GetReportDataPagedAsync(relReport, from, to, Connect);
        }
        
        public System.Data.DataSet GetDataSetList(string[][] sqlParList) {
            return base.Channel.GetDataSetList(sqlParList);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetDataSetListAsync(string[][] sqlParList) {
            return base.Channel.GetDataSetListAsync(sqlParList);
        }
    }
}
