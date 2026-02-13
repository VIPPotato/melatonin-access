 = Get-Content Loc.cs
 = @()
for ( = 0;  -lt .Count; ++) {
    if ([].Trim() -eq 'Add(') {
         = @()
         =  + 1
        while ( -lt .Count) {
             = [].Trim()
            if ( -eq '') { ++; continue }
            if (.StartsWith([char]34)) {
                 = .LastIndexOf([char]34)
                 += .Substring(1,  - 1)
