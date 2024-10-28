# 语法分析作业文档

姓名: 李昊轩
学号: 2021012900
邮箱: lihaoxua21@mails.tsinghua.edu.cn

## 运行方式

在 `bin` 目录下，提供了可执行文件 `ToyAnalyzer.exe`，可以直接运行。
程序可以接受一个参数，为源代码文件的路径。
若程序没有接收到参数，则会从标准输入读取源代码。

```shell
./bin/ToyAnalyzer.exe <path-to-source-file>
```

## 测试样例

样例在 `testcases` 目录下，包含了 3 个测试样例。

第一个样例测试是摄氏度转华氏度的程序，测试了基本语句和符号优先级的情况。

![test1](../testcases/1.in)
![test1](../testcases/1.out)

第二个样例测试了条件语句嵌套的情况。

![test2](../testcases/2.in)
![test2](../testcases/2.out)

第三个样例是计算阶乘的程序，是一个综合性的测试。

![test3](../testcases/3.in)
![test3](../testcases/3.out)

#### 附：LL(1) 语法规则

![lexer_rules](../src/ToyAnalyzer/Config/grammar_rules.json)
