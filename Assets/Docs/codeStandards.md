# Various Code Standards
This document describes the various code standards for style and formatting consistency.  

This is a living document. Add and update standards as desired.

## Prefix `private` variables with an `_`underscore
```
private string _parts;
```
## Private variables can be `serialized`
```
[SerializeField]
private string _parts;
```

## Encapsulate private variables with public accessors
```
private string _parts;
public string parts { get => _parts; set => _parts = value; }
```

## Avoid nesting with a `short circuit`
*`short circuit` = `return` if we don't have the `thing` we need.*

**DO THIS:**   
```
public void check()
{
  if (!thing) return;

  doStuff();
}
```
**NOT THIS:**  
```
public void check()
{
  if (thing) {
    doStuff();
  }
}
```
## Keep `Update()` concise by extracting logic into separate functions
*Functions names describe intent and establish a separation of concerns*

**DO THIS:**   
```
private void Update() 
{
  Walk();
}

private void Walk()
{
  float xInput = Input.GetAxis("Horizontal");
  float yInput = Input.GetAxis("Vertical");

  movementDirection = new Vector3(xInput, 0, yInput).normalized;
}
```
**NOT THIS:**  
```
private void Update() 
{
  float xInput = Input.GetAxis("Horizontal");
  float yInput = Input.GetAxis("Vertical");

  movementDirection = new Vector3(xInput, 0, yInput).normalized;
}
```


## TEMPLATE
**DO THIS:**   
```
```
**NOT THIS:**  
```
```
