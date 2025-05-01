# ⚡💡 LogicCircuits 🔌 (Screenshots are below)

**LogicCircuits** is a Windows desktop application (C#, .NET Framework, Windows Forms) that provides an interactive graphical environment for designing and analyzing combinational logic circuits.

Users can drag-and-drop standard logic gates (AND, OR, XOR, NAND, etc.) onto a canvas and connect them with wires to form complete circuits. Each gate implements a fundamental Boolean function, combining binary inputs to produce an output.

Input and output nodes can be given custom labels, and as the user toggles input signals, the application propagates changes through the network in real time, dynamically updating every output. This instant feedback allows users to verify circuit behavior immediately.

LogicCircuits then automatically generates the complete **truth table** for the assembled circuit, listing all combinations of input values and corresponding outputs. From this truth table, the application derives both the **Disjunctive Normal Form (DNF)** and **Conjunctive Normal Form (CNF)** of the Boolean function.    
  
![image](https://github.com/user-attachments/assets/af6e8fb4-5662-4f29-9e9a-e4ede6810864)


These are canonical forms:
- **DNF** = OR of AND-clauses
- **CNF** = AND of OR-clauses

They fully characterize the logic function.

For further analysis, LogicCircuits implements the **Quine–McCluskey algorithm** — a tabulation method for minimization of Boolean functions — to simplify the derived expressions.

All circuit designs can be saved to disk and reloaded later via .NET serialization. The UI uses optimized painting (partial and full redraw) to maintain smooth performance even for complex circuits.



https://github.com/user-attachments/assets/895d0657-0576-437b-8d89-d96c1e87374a



---

## ✨ Features

### 🖱️ Interactive Circuit Design
- Drag-and-drop a variety of logic gate symbols (AND, OR, XOR, NAND, NOR, XNOR, NOT, BUF, etc.) from the toolbox onto a canvas.
- Click and connect gate input/output ports with wires to build custom circuits.
- Rename any input or output node by double-clicking the label.


https://github.com/user-attachments/assets/c9f76435-73a6-431a-8793-065bc70789bc



### ⚡ Real-Time Simulation
- Change input signal values (true/false or 1/0) at any time.
- The application propagates these changes through the circuit immediately, updating all gate outputs on-the-fly.
- This ensures that output values always reflect the current input state.

### 📊 Truth Table Generation
- Automatically computes the full **truth table** for the constructed logic circuit.
- The truth table is a mathematical table used in logic to represent all possible input combinations and their resulting outputs.
- It provides a complete functional description of the circuit’s behavior.


https://github.com/user-attachments/assets/4d49256d-71fd-4020-84b9-f23f268b755d


### 🧾 DNF / CNF Expression Output
- From the truth table, the application generates the circuit’s **Disjunctive Normal Form (DNF)** and **Conjunctive Normal Form (CNF)** expressions.
- In Boolean logic:
  - DNF is a **disjunction of conjunctions** (OR of AND-terms),
  - CNF is a **conjunction of disjunctions** (AND of OR-terms).
- These expressions explicitly represent the logical function computed by the circuit.

### 🧮 Boolean Minimization
- Applies the **Quine–McCluskey algorithm** to minimize the Boolean expressions derived from the circuit.
- This systematic tabulation method finds a minimal sum-of-products (or product-of-sums) form.
- The minimized expression is useful for optimization and analysis.

### 💾 Persistence (Save/Load)
- Save complete circuit configurations (including gate layout, connections, and node labels) to a file via .NET serialization.
- Circuits can be reloaded exactly as they were, enabling users to resume work or share designs easily.

### 🎨 Optimized Rendering
- The graphical interface employs **partial and full redraw** techniques.
- Only changed portions of the canvas are repainted when a gate output changes or a new element is added.
- This ensures smooth user experience and efficient performance, even for larger circuits.

---

## 🏗️ Architecture and Design

LogicCircuits is built with a clear **object-oriented architecture**.

All elements in the circuit implement a common `IElement` interface, ensuring they provide basic functionality such as value evaluation and drawing.

Logic gates implement an `IGate` interface, which itself extends:
- `IInputContainingElement`
- `IOutputContainingElement`  

![image](https://github.com/user-attachments/assets/fb7390f4-c523-4f17-a330-df00d42b3c2b)


These interfaces define how elements handle input and output connections, respectively.

Each specific gate type (AND, OR, XOR, NAND, NOR, XNOR, etc.) is implemented as a class inheriting from `IGate`. These gate classes encapsulate their logical behavior and override the `CalculateValue()` method to compute their output from inputs (e.g., an AND gate returns true only if all its inputs are true).

Input nodes and output nodes are implemented as separate classes that provide or consume values and implement the appropriate interfaces.

---
![Screenshot 2025-05-01 195428](https://github.com/user-attachments/assets/bda9ad6e-026a-4d62-941f-6c0a0cc22db1)

### 🔑 Key Interfaces and Classes

- `IElement`: Base interface for any circuit element (gates, inputs, outputs, etc.).
- `IGate`: Extends `IInputContainingElement` and `IOutputContainingElement`; implemented by all gate classes.
- **Gate Classes**: `AndGate`, `OrGate`, `XorGate`, `NandGate`, etc. — each inherits from `IGate` and defines its Boolean logic.
- **Input/Output Elements**: `InputNode` and `OutputNode` classes that hold and provide signal values; they implement interfaces for getting/setting values.

---
![Screenshot 2025-05-01 195450](https://github.com/user-attachments/assets/be915ff6-61b3-4446-a046-834e06532ee2)

### 🔁 Value Calculation

- The `CalculateValue()` method on each element is called recursively.
- When an element’s value is needed, it requests values from its input connections (which may be other gates or primary inputs).
- This recursive evaluation continues until the primary inputs are reached, producing a final output.
- This design mirrors how Boolean logic is composed and evaluated in hardware or simulation.

---
![Screenshot 2025-05-01 195511](https://github.com/user-attachments/assets/9a97cce0-824e-4504-9df6-21d4d442d102)

### 🖼️ Rendering Engine

- The UI components handle drawing each element on the Windows Forms canvas.
- When inputs change or the circuit is edited:
  - The program invalidates only affected regions and repaints accordingly (**partial redraw**).
  - A **full redraw** is used when larger changes occur (e.g., window resizing or major layout updates).
- This optimization maintains responsiveness and performance.

---

> References:
> - [Boolean Function – Wikipedia](https://en.wikipedia.org/wiki/Boolean_function)
> - [Truth Table – Wikipedia](https://en.wikipedia.org/wiki/Truth_table)
> - [Disjunctive Normal Form – Wikipedia](https://en.wikipedia.org/wiki/Disjunctive_normal_form)
> - [Conjunctive Normal Form – Wikipedia](https://en.wikipedia.org/wiki/Conjunctive_normal_form)
> - [Quine–McCluskey Algorithm – Wikipedia](https://en.wikipedia.org/wiki/Quine%E2%80%93McCluskey_algorithm)

---
