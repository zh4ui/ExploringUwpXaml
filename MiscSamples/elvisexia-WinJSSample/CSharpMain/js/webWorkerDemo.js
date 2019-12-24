let str = "hello from worker"
// if uncommented, the blow line throws "JavaScript runtime error: Unable to get property 'Example' of undefined or null reference"
// str = self.MyRuntimeComponent.Example.getAnswer();
postMessage(str);
