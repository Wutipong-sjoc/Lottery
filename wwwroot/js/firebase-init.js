// =============================
// 📁 firebase-init.js
// =============================

// ✅ Import เฉพาะ module ที่จำเป็นสำหรับการเริ่มต้น Firebase
import { initializeApp } from "https://www.gstatic.com/firebasejs/12.5.0/firebase-app.js";
import { getAuth } from "https://www.gstatic.com/firebasejs/12.5.0/firebase-auth.js";
import { getDatabase } from "https://www.gstatic.com/firebasejs/12.5.0/firebase-database.js";

// ✅ ตั้งค่า Firebase Project ของคุณ (จาก Firebase Console)
const firebaseConfig = {
    apiKey: "AIzaSyBrRDITShx87uOiV53pbUV-gSyUsop2eCc",
    authDomain: "lottery-624e2.firebaseapp.com",
    databaseURL: "https://lottery-624e2-default-rtdb.asia-southeast1.firebasedatabase.app",
    projectId: "lottery-624e2",
    storageBucket: "lottery-624e2.appspot.com",
    messagingSenderId: "883271265085",
    appId: "1:883271265085:web:22be81f98110a78ce7f354"
};

// ✅ สร้าง instance ของ Firebase
const app = initializeApp(firebaseConfig);

// ✅ สร้าง instance ของ Auth และ Database
const auth = getAuth(app);
const db = getDatabase(app);

// ✅ ส่งออก (export) ให้ไฟล์อื่นใช้ได้
export { app, auth, db };