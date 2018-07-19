
Write-Host "Testing Validation_Approval.exe"


$test1 = & .\FileSystem\bin\Debug\Validation_Approval.exe -D  ./Test1 -C -A B,D -Ch ./Test1/Dir3/file3.txt

if( $test1 -like "*Approved."){
    Write-Host "One Level Dependency Passed!"
}else{
    Write-Host "One Level Dependency Failed!"
    Write-Host $test1
}


$test2 = & .\FileSystem\bin\Debug\Validation_Approval.exe -D  ./Test1 -C -A B,F -Ch ./Test1/Dir3/file3.txt

if( $test2 -like "*Insufficient Approval."){
    Write-Host "Two Level Dependency missing passed!"
}else{
    Write-Host "Two Level Dependency missing Failed!"
    Write-Host $test2
}

$test3 = & .\FileSystem\bin\Debug\Validation_Approval.exe -D  ./Test1 -C -A A,F -Ch ./Test1/Dir2/file3.txt

if( $test3 -like "*Approved."){
    Write-Host "Two Level Dependency passed!"
}else{
    Write-Host "Two Level Dependency Failed!"
    Write-Host $test3
}

$test4 = & .\FileSystem\bin\Debug\Validation_Approval.exe -D  ./Test2 -C -A A,F -Ch ./Test1/Dir2/file3.txt

if( $test4 -like "*Circular Dependency Detected."){
    Write-Host "Circular Dependency Check passed!"
}else{
    Write-Host "Circular Dependency Check Failed!"
    Write-Host $test4
}