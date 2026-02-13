import re
lines=open('Loc.cs','r',encoding='utf-8').read().splitlines()
entries=[]
i=0
while i<len(lines):
    if 'Add(' in lines[i] and 'private static void Add' not in lines[i]:
        block=[]
        i+=1
        while i<len(lines) and ');' not in lines[i]:
            block.append(lines[i])
            i+=1
        block.append(lines[i])
        entries.append(block)
    i+=1
for block in entries:
    strings=[]
    for line in block:
        stripped=line.strip()
        if stripped.startswith(' ):
            end=stripped.rfind( ')
            if end>0:
                strings.append(stripped[1:end])
    if len(strings)==11:
        key, fr, de, es, pt = strings[0], strings[6], strings[7], strings[8], strings[9]
        print(key + '\t' + fr + '\t' + de + '\t' + es + '\t' + pt)
    else:
        print('unexpected', len(strings))
