# 词法分析作业文档

姓名: 李昊轩
学号: 2021012900
邮箱: lihaoxua21@mails.tsinghua.edu.cn

## 项目简介

本项目由 C# 编写，实现了 toy 语言的词法分析器。对于源代码输入，可以输出 token 序列。

通过正则表达式匹配，实现了对 toy 语言的关键字、标识符、常数、运算符、界符等的识别。
对 token 进行了分类，设置匹配时的**优先级**，可以正确处理关键字和标识符的冲突。

词法规则（正则表达式）保存在 json 文件中，可以通过修改 json 文件来容易地修改词法规则。但在发布时，将 json 文件打包进了可执行文件中，因此无法直接修改。在文档的最后，提供了 json 文件的内容。

关于负数的处理，由于在词法分析阶段无法确定负号是运算符还是常数的一部分，因此决定将所有的负号视为运算符，在语法分析阶段再处理。

## 运行方式

在 `bin` 目录下，提供了可执行文件 `ToyAnalyzer.exe`，可以直接运行。

```shell
./bin/ToyAnalyzer.exe <input_file>
```

## 测试样例

样例在 `testcases` 目录下，包含了 3 个测试样例。

第一个样例测试了基本的关键字、标识符、常数、运算符、界符的识别。

![test1](../testcases/1.in)
![test1](../testcases/1.out)

第二个样例测试了一些特殊的情况，如关键字和标识符的冲突，连续的语句和符号等。

![test2](../testcases/2.in)
![test2](../testcases/2.out)

第三个样例是计算阶乘的程序，测试了条件和循环语句嵌套的情况。

![test3](../testcases/3.in)
![test3](../testcases/3.out)

#### 附：文法规则

![lexer_rules](../src/ToyAnalyzer/Config/lexer_rules.json)
