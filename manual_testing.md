Updated with **Windows PC** instead of macOS:

---

**Manual Testing Summary**  
**Project:** HR Management System – Amader IT (Demo)  
**Test URL:** [https://amaderit.net/demo/hr](https://amaderit.net/demo/hr)  
**Date:** May 1, 2025  
**Tester:** Muammar Tazwar Asfi  
**Test Type:** Functional & UI  

**Access:**

* User ID: 12345678
* Password: 19970204

---

### 1. Test Environment

* **Devices:** Windows PC, Honor Pro 200
* **OS:** Windows 11, Android 14
* **Browsers:** Chrome
* **Resolutions:** 2560x1440, 412x915
* **Network:** Office LAN (50 Mbps)

---

### 2. Test Goals

* Check core HR functions (login, dashboard, employee, leave, attendance, payroll)
* Confirm UI elements load and behave properly
* Validate forms and input rules
* Test responsiveness on desktop and mobile
* Assess error handling and logout

---

### 3. Key Test Cases

| ID    | Scenario                   | Result                    |
| ----- | -------------------------- | ------------------------- |
| TC-01 | Valid login                | ✅ Pass                    |
| TC-02 | Invalid login              | ✅ Pass                    |
| TC-03 | Dashboard access           | ✅ Pass                    |
| TC-04 | View employee list         | ✅ Pass                    |
| TC-05 | Apply for leave            | ✅ Pass                    |
| TC-06 | View attendance report     | ✅ Pass                    |
| TC-07 | Leave form validation      | ✅ Pass                    |
| TC-08 | Logout flow                | ✅ Pass                    |
| TC-09 | Mobile view responsiveness | ✅ Pass (minor UI overlap) |

---

### 4. Known Issues

| ID     | Description                       | Severity |
| ------ | --------------------------------- | -------- |
| UI-001 | Footer elements overlap on mobile | Low      |

---

### 5. Suggestions

* Fix footer spacing on mobile
* Add loading indicators when switching modules
* Improve accessibility with ARIA labels

---

### 6. Final Notes

The system is stable with all core features working as expected. Just a few minor UI adjustments recommended before release.

