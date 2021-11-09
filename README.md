## 用途：检查域名SSL证书是否过期

### 效果如下
```bash
===============cert info======================
Subject: CN=trojan001.southeastasia.cloudapp.azure.com
Issuer: CN=R3, O=Let's Encrypt, C=US
Algorithm: sha256RSA
Cert is valid from: 2021/08/08 to: 2021/11/06
===============================================
Careful,your domain(trojan001.southeastasia.cloudapp.azure.com)'s ssl cert will expire within 1 month

```

## TODOs：
- [x] 根据域名自动下载证书内容
- [x] 打印证书基本信息
- [x] 计算&打印到期天数
- [ ] 支持在命令行中指定域名及接收提醒的邮箱
- [ ] 通过github action workflow 每日自动执行，对配置的域名进行检测，如果检测临近到期时，发送邮件通知
