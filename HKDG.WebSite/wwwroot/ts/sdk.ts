import { InstoreSdk } from "./instoreSdk"


declare global {
    interface Window {
        InstoreSdk:InstoreSdk;
    }
}
 


window.InstoreSdk=new InstoreSdk();
 