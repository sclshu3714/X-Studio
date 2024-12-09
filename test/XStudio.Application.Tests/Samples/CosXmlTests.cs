using System;
using XStudio;
using XStudio.Common.Clouds;
using XStudio.Common.Helper;
using XStudio.Samples;
using Xunit;

namespace XStudio.Samples;
public class CosXmlTests{
    // 添加一个TestMethod测试AccessKey与SecretKey加密与解密
    [Fact]
    public void Test_AccessKeyAndSecretKeyEncryptionDecryption() {
        // 测试加密
        string originalAccessKey = "AKIDiz73lR0Tkuvp8Rel0FNMDtkPxOfsy33y";
        string originalSecretKey = "unXAczJYpy462vokZFqQ0TrBYxIYp6KU";
        string originalAppId = "1304449501"; // 加密密钥
        EncrypterHelper.EncryptionKey = "AIC_TENCENT_CLOUD_COS_ENCRYPTION_KEY"; // 确保加密密钥正确
        string encryptedAccessKey = EncrypterHelper.Encrypt(originalAccessKey);
        string encryptedSecretKey = EncrypterHelper.Encrypt(originalSecretKey);
        string encryptedAppId = EncrypterHelper.Encrypt(originalAppId);

        // 测试解密
        string decryptedAccessKey = EncrypterHelper.Decrypt(encryptedAccessKey);
        string decryptedSecretKey = EncrypterHelper.Decrypt(encryptedSecretKey);
        string decryptedAppId = EncrypterHelper.Decrypt(encryptedAppId);

        // 验证
        Assert.Equal(originalAccessKey, decryptedAccessKey);
        Assert.Equal(originalSecretKey, decryptedSecretKey);
        Assert.Equal(originalAppId, decryptedAppId);
    }

    // 测试正常情况
    [Fact]
    public void Test_InitCosXml_HappyPath() {
        // 初始化
        var instance = new TencentCloudClient(); // 替换为包含 InitCosXml 方法的类名
        instance.AccessKey = "qo5CnkhITrizTGjgONg3ua+YLgJW35vufUW79Xm5wpRGa+nEZIzbGZnRtjpIpoPQ";
        instance.SecretKey = "nqtTm0JIM9KvZ2yCYdsuos2MOj9WzafhYEmZ/HHA77IvSMqteJDHBYXNqiZUup/M";
        Environment.SetEnvironmentVariable("COS_REGION", "ap-chengdu");
        Environment.SetEnvironmentVariable("SECRET_ID", "qo5CnkhITrizTGjgONg3ua+YLgJW35vufUW79Xm5wpRGa+nEZIzbGZnRtjpIpoPQ");
        Environment.SetEnvironmentVariable("SECRET_KEY", "nqtTm0JIM9KvZ2yCYdsuos2MOj9WzafhYEmZ/HHA77IvSMqteJDHBYXNqiZUup/M");

        // 执行方法
        instance.InitCosXml();

        // 验证
        Assert.NotNull(instance.cosXml);
    }

    // 测试区域为空情况
    [Fact]
    public void Test_InitCosXml_RegionNull() {
        // 初始化
        var instance = new TencentCloudClient();
        instance.AccessKey = "qo5CnkhITrizTGjgONg3ua+YLgJW35vufUW79Xm5wpRGa+nEZIzbGZnRtjpIpoPQ";
        instance.SecretKey = "nqtTm0JIM9KvZ2yCYdsuos2MOj9WzafhYEmZ/HHA77IvSMqteJDHBYXNqiZUup/M";
        Environment.SetEnvironmentVariable("COS_REGION", null); // 设置区域为空

        // 执行方法
        instance.InitCosXml();

        // 验证
        Assert.NotNull(instance.cosXml);
    }

    // 测试秘钥环境变量为空情况
    [Fact]
    public void Test_InitCosXml_SecretIdAndKeyNull() {
        // 初始化
        var instance = new TencentCloudClient();
        instance.AccessKey = "qo5CnkhITrizTGjgONg3ua+YLgJW35vufUW79Xm5wpRGa+nEZIzbGZnRtjpIpoPQ";
        instance.SecretKey = "nqtTm0JIM9KvZ2yCYdsuos2MOj9WzafhYEmZ/HHA77IvSMqteJDHBYXNqiZUup/M";
        Environment.SetEnvironmentVariable("SECRET_ID", null);
        Environment.SetEnvironmentVariable("SECRET_KEY", null);

        // 执行方法
        instance.InitCosXml();

        // 验证
        Assert.NotNull(instance.cosXml);
        // 验证使用 AccessKey 和 SecretKey 作为秘钥
    }

    // 测试秘钥环境变量解密
    [Fact]
    public void Test_InitCosXml_SecretIdAndKeyEncrypted() {
        // 初始化
        var instance = new TencentCloudClient();
        instance.AccessKey = "qo5CnkhITrizTGjgONg3ua+YLgJW35vufUW79Xm5wpRGa+nEZIzbGZnRtjpIpoPQ";
        instance.SecretKey = "nqtTm0JIM9KvZ2yCYdsuos2MOj9WzafhYEmZ/HHA77IvSMqteJDHBYXNqiZUup/M";
        EncrypterHelper.EncryptionKey = "AIC_TENCENT_CLOUD_COS_ENCRYPTION_KEY"; // 确保加密密钥正确
        string encryptedId = EncrypterHelper.Encrypt("AKIDiz73lR0Tkuvp8Rel0FNMDtkPxOfsy33y");
        string encryptedKey = EncrypterHelper.Encrypt("unXAczJYpy462vokZFqQ0TrBYxIYp6KU");

        Environment.SetEnvironmentVariable("SECRET_ID", encryptedId);
        Environment.SetEnvironmentVariable("SECRET_KEY", encryptedKey);

        // 执行方法
        instance.InitCosXml();

        // 验证
        Assert.NotNull(instance.cosXml);
        // 根据解密后的结果进行进一步验证
    }
}
