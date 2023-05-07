Extract documentation

```
grep -P "private|public|static|class|///" Program.cs | grep -vP "///.*(summary|param name|returns|exception)" > doc.txt
```

Reformat in notepad++:
Find: /// (.*)\n(.*)
Replace: \2 - \1